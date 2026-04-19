using LMS.Api.DTOs.Auth;
using LMS.DTOs.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LMS.Application.Services.AuthServices;

namespace LMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // =========================
        // REGISTER
        // =========================
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }

        // =========================
        // VERIFY EMAIL OTP
        // =========================
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyOtpDto dto)
        {
            var result = await _authService.VerifyEmailAsync(dto);
            return Ok(result);
        }

        // =========================
        // LOGIN
        // =========================
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }

        // =========================
        // FORGOT PASSWORD (SEND OTP)
        // =========================
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var result = await _authService.ForgotPasswordAsync(email);
            return Ok(result);
        }

        // =========================
        // VERIFY RESET OTP
        // =========================
        [HttpPost("verify-reset")]
        public async Task<IActionResult> VerifyReset(VerifyOtpDto dto)
        {
            var result = await _authService.VerifyResetAsync(dto);
            return Ok(result);
        }

        // =========================
        // RESET PASSWORD
        // =========================
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetForgetPasswordDto dto)
        {
            var result = await _authService.ResetForgetPassword(dto);
            return Ok(result);
        }

        // =========================
        // REFRESH TOKEN
        // =========================
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);
            return Ok(result);
        }

        // =========================
        // LOGOUT
        // =========================
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenDto dto)
        {
            await _authService.LogoutAsync(dto);
            return Ok("Logged out successfully");
        }

        // =========================
        // CHANGE PASSWORD
        // =========================
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(string userId, ChangePasswordDto dto)
        {
            await _authService.ChangePasswordAsync(userId, dto);
            return Ok("Password changed successfully");
        }

        // =========================
        // OAUTH LOGIN
        // =========================
        [HttpGet("login/facebook")]
        public IActionResult FacebookLogin(string? returnUrl)
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalCallback", "Auth"),
                Items = { { "returnUrl", returnUrl ?? "" } }
            };

            return Challenge(props, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet("login/google")]
        public IActionResult GoogleLogin(string? returnUrl)
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalCallback", "Auth"),
                Items = { { "returnUrl", returnUrl ?? "" } }
            };

            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("external-callback")]
        public async Task<IActionResult> ExternalCallback()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

            if (!result.Succeeded)
                return Unauthorized("External login failed");

            var email =
                result.Principal?.FindFirst(ClaimTypes.Email)?.Value ??
                result.Principal?.FindFirst("email")?.Value;

            if (string.IsNullOrEmpty(email))
                return BadRequest("Email not provided by provider");

            var response = await _authService.HandleOAuthLoginAsync(email);

            var returnUrl = result.Properties?.Items.TryGetValue("returnUrl", out var url) == true
                ? url
                : "http://localhost:3000/external-callback";

            var redirectUrl =
                $"{returnUrl}?accessToken={Uri.EscapeDataString(response.AccessToken)}" +
                $"&refreshToken={Uri.EscapeDataString(response.RefreshToken)}";

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return Redirect(redirectUrl);
        }
        // =========================
        // ASSIGN ROLE (ADMIN ONLY)
        // =========================
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(string email, string role)
        {
            await _authService.AssignRoleAsync(email, role);
            return Ok("Role assigned successfully");
        }
    }
}