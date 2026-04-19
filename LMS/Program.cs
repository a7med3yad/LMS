using LMS.Api.Services;
using LMS.Application.Services.AuthServices;
using LMS.Common;
using LMS.Data;
using LMS.Infrastructure.Services;
using LMS.Models;
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
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<OtpService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<OAuthService>();
#endregion

#region Email Settings
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
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