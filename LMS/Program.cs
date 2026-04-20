using LMS.Api.Services;
using LMS.Application.Services;
using LMS.Application.Services.AuthServices;
using LMS.Application.Services.Interfaces;
using LMS.Common;
using LMS.Data;
using LMS.Infrastructure.Services;
using LMS.Models;
using LMS.Repositories;
using LMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

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

#region Authentication (FIXED - NO DUPLICATION)
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
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Application Services
builder.Services.AddScoped<AuthService>();                               // ✅ DONE
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Infrastructure Services
builder.Services.AddScoped<TokenService>();                              // ✅ DONE
builder.Services.AddScoped<OtpService>();                                // ✅ DONE
builder.Services.AddScoped<EmailService>();                              // ✅ DONE
builder.Services.AddScoped<OAuthService>();                              // ✅ DONE
builder.Services.AddScoped<IStripeService, StripeService>();

#endregion

#region Settings
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<StripeSettings>(
    builder.Configuration.GetSection("Stripe"));
#endregion

var app = builder.Build();

#region Middleware Pipeline

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();   // MUST be before Authorization
app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();