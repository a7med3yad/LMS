using LMS.Api.Middleware;
using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Behaviors;
using LMS.Application.Command.Course;
using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Services;
using LMS.Application.Services.AuthServices;
using LMS.Domain.Abstraction.Repositories;
using LMS.Domain.Common;
using LMS.Domain.Models;
using LMS.Domain.Services;
using LMS.Infrastructure.Dapper;
using LMS.Infrastructure.Persistence;
using LMS.Infrastructure.Repositories;
using LMS.Infrastructure.Services;
using LMS.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

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
#region DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
#endregion

#region Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

    options.CallbackPath = "/signin-google";

    options.Scope.Add("email");
    options.Scope.Add("profile");

    options.SaveTokens = true;
})
.AddFacebook(options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];

    options.CallbackPath = "/signin-facebook";

    options.Fields.Add("email");
    options.Fields.Add("name");

    options.SaveTokens = true;
});
// ============================ AUTHORIZATION ============================
builder.Services.AddAuthorization();

// ============================ DAPPER ============================
builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<ISqlQueryService, SqlQueryService>();

// ============================ UNIT OF WORK ============================
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ============================ REPOSITORIES ============================
#endregion

#region Authorization
builder.Services.AddAuthorization();
#endregion

#region Controllers + OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();
#endregion

#region Dependency Injection

// Repositories
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
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Application Services
builder.Services.AddScoped<AuthService>();

// Infrastructure Services
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<OtpService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<OAuthService>();
builder.Services.AddScoped<IStripeService, StripeService>();

// ============================ SETTINGS ============================
#endregion

#region Settings
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<StripeSettings>(
    builder.Configuration.GetSection("Stripe"));

// ============================ CONTROLLERS + OPENAPI ============================
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ============================ APP ============================
var app = builder.Build();

// ============================ MIDDLEWARE ============================
app.UseMiddleware<ExceptionMiddleware>();

// Enable OpenAPI + Scalar in ALL environments
app.MapOpenApi();
app.MapScalarApiReference();

// Handle reverse proxy (important for hosting)
#endregion

var app = builder.Build();

#region Middleware Pipeline

// ✅ Enable OpenAPI + Scalar in ALL environments
app.MapOpenApi();
app.MapScalarApiReference();

// ✅ Handle reverse proxy (important for hosting)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                       ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();

app.UseAuthentication();
// ⚠️ Keep only if HTTPS works on your host
app.UseHttpsRedirection();

app.UseAuthentication();   // MUST be before Authorization
app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();