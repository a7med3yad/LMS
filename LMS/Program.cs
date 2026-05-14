using Amazon.S3;
using LMS.Api.Middleware;
using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Behaviors;
using LMS.Application.Command.Course;
using LMS.Application.Services;
using LMS.Application.Services.AuthServices;
using LMS.Domain.Abstraction.Repositories;
using LMS.Domain.Common;
using LMS.Domain.Models;
using LMS.Domain.Services;
using LMS.Infrastructure.Dapper;
using LMS.Infrastructure.Helper;
using LMS.Infrastructure.Persistence;
using LMS.Infrastructure.Repositories;
using LMS.Infrastructure.Services;
using LMS.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ============================ DATABASE ============================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ============================ IDENTITY ============================
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// ============================ AUTHENTICATION ============================
var authBuilder = builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ClockSkew = TimeSpan.Zero
    };
})
.AddCookie(options =>
{
    // Prevent cookie auth from redirecting API requests — return 401 JSON instead
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

// ==== Google OAuth (only if credentials are configured) ====
var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
if (!string.IsNullOrEmpty(googleClientId) && !string.IsNullOrEmpty(googleClientSecret))
{
    authBuilder.AddGoogle(options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;

        options.CallbackPath = "/signin-google";

        options.Scope.Add("email");
        options.Scope.Add("profile");

        options.SaveTokens = true;
    });
}

// ==== Facebook OAuth (only if credentials are configured) ====
var fbAppId = builder.Configuration["Authentication:Facebook:AppId"];
var fbAppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
if (!string.IsNullOrEmpty(fbAppId) && !string.IsNullOrEmpty(fbAppSecret))
{
    authBuilder.AddFacebook(options =>
    {
        options.AppId = fbAppId;
        options.AppSecret = fbAppSecret;

        options.CallbackPath = "/signin-facebook";

        options.Fields.Add("email");
        options.Fields.Add("name");

        options.SaveTokens = true;
    });
}

// ============================ AUTHORIZATION ============================
builder.Services.AddAuthorization();

// ============================ CORS ============================
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var origins = new List<string>
        {
            "http://localhost:3000",
            "http://localhost:3001",
            "https://localhost:3000",
            "http://localhost:5173", // Vite dev
        };

        // Add production frontend URL if configured
        var frontendUrl = builder.Configuration["FrontendUrl"];
        if (!string.IsNullOrEmpty(frontendUrl))
            origins.Add(frontendUrl);

        policy.WithOrigins(origins.ToArray())
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ============================ DAPPER ============================
builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<ISqlQueryService, SqlQueryService>();

// ============================ UNIT OF WORK ============================
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ============================ REPOSITORIES ============================
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// ============================ MEDIATR ============================
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateCourseCommand>();
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
});

// ============================ APPLICATION SERVICES ============================
builder.Services.AddScoped<AuthService>();

// ============================ INFRASTRUCTURE SERVICES ============================
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<OtpService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<OAuthService>();
builder.Services.AddScoped<IStripeService, StripeService>();

// ============================ SETTINGS ============================
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<StripeSettings>(
    builder.Configuration.GetSection("Stripe"));
builder.Services.Configure<OciSettings>(
    builder.Configuration.GetSection("OracleObjectStorage"));

// ============================ ORACLE OCI OBJECT STORAGE ============================
var ociSettings = builder.Configuration.GetSection("OracleObjectStorage").Get<OciSettings>();
if (ociSettings != null && !string.IsNullOrEmpty(ociSettings.AccessKeyId))
{
    builder.Services.AddSingleton<IAmazonS3>(sp =>
    {
        var config = new AmazonS3Config
        {
            ServiceURL = ociSettings.S3Endpoint,
            ForcePathStyle = true, // Required for OCI S3-compatible API
        };
        return new AmazonS3Client(
            ociSettings.AccessKeyId,
            ociSettings.SecretAccessKey,
            config);
    });
    builder.Services.AddScoped<IFileStorageService, OciStorageService>();
}

// ============================ CONTROLLERS + OPENAPI ============================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Accept enum values as strings (e.g. "Student" instead of 0)
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();

// ============================ APP ============================
var app = builder.Build();

// ============================ MIDDLEWARE ============================
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ExceptionMiddleware>();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                       ForwardedHeaders.XForwardedProto
});

// ── CORS must come BEFORE auth and BEFORE HTTPS redirect ──
app.UseCors();

// Only redirect to HTTPS in production (prevents preflight failures in dev)
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ============================ HEALTH CHECK ============================
app.MapGet("/health", () => Results.Ok(new
{
    Status = "Healthy",
    Timestamp = DateTime.UtcNow,
    Environment = app.Environment.EnvironmentName
}));

// ============================ SEED ROLES ============================
await RoleSeeder.SeedRolesAsync(app.Services);

app.Run();