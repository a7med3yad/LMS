using LMS.Application.Api.DTOs.Auth;
using LMS.Application.DTOs.Auth;
using LMS.Domain.Models;
using LMS.Domain.Models.Enums;
using LMS.Infrastructure.Persistence;
using LMS.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS.Application.Services.AuthServices
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly TokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly OtpService _otpService;
        private readonly AppDbContext _context;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            TokenService tokenService,
            EmailService emailService,
            OtpService otpService,
            AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _otpService = otpService;
            _context = context;
        }

        // =========================
        // REGISTER
        // =========================
        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("Email already registered");

            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                Role = dto.Role == UserRole.Instructor ? UserRole.Instructor : UserRole.Student,
                FullName = dto.FullName,
                IsVerified = false
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "User");

            if (!_otpService.CanRequestOtp(user))
                throw new Exception("Too many OTP requests. Please wait.");

            var otp = _otpService.GenerateOtp();

            user.OtpExpiration = _otpService.GetExpiration();

            await _userManager.UpdateAsync(user);

            // store OTP
            await _userManager.SetAuthenticationTokenAsync(user, "AuthApi", "OTP", otp);

            await _emailService.SendOtpAsync(user.Email!, otp);

            return "OTP sent to email.";
        }

        // =========================
        // VERIFY EMAIL OTP
        // =========================
        public async Task<AuthResponseDto> VerifyEmailAsync(VerifyOtpDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email)
                ?? throw new Exception("User not found");

            if (_otpService.IsExpired(user.OtpExpiration))
                throw new Exception("OTP expired");

            var storedOtp = await _userManager.GetAuthenticationTokenAsync(user, "AuthApi", "OTP");

            if (storedOtp != dto.Otp)
                throw new Exception("Invalid OTP");

            user.IsVerified = true;
            user.OtpExpiration = null;

            await _userManager.UpdateAsync(user);
            await _userManager.RemoveAuthenticationTokenAsync(user, "AuthApi", "OTP");

            var accessToken = await _tokenService.CreateAccessToken(user);
            var refreshToken = await _tokenService.CreateRefreshToken(user);

            return new AuthResponseDto(
                accessToken,
                refreshToken.Token,
                user.Id.ToString(),
                user.FullName,
                user.Email,
                (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User"
            );
        }

        // =========================
        // LOGIN
        // =========================
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email)
                ?? throw new Exception("User not found");

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new Exception("Invalid password");

            if (!user.IsVerified)
                throw new Exception("Email not verified");

            var accessToken = await _tokenService.CreateAccessToken(user);
            var refreshToken = await _tokenService.CreateRefreshToken(user);

            return new AuthResponseDto(
                accessToken,
                refreshToken.Token,
                user.Id.ToString(),
                user.FullName,
                user.Email,
                (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User"
            );
        }

        // =========================
        // FORGOT PASSWORD (SEND OTP)
        // =========================
        public async Task<string> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new Exception("User not found");

            if (!_otpService.CanRequestOtp(user))
                throw new Exception("Too many OTP requests");

            var otp = _otpService.GenerateOtp();

            user.OtpExpiration = _otpService.GetExpiration();

            await _userManager.UpdateAsync(user);

            await _userManager.SetAuthenticationTokenAsync(user, "AuthApi", "ResetOTP", otp);

            await _emailService.SendOtpAsync(email, otp);

            return "OTP sent.";
        }

        // =========================
        // VERIFY RESET OTP
        // =========================
        public async Task<AuthResponseDto> VerifyResetAsync(VerifyOtpDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email)
                ?? throw new Exception("User not found");

            if (_otpService.IsExpired(user.OtpExpiration))
                throw new Exception("OTP expired");

            var storedOtp = await _userManager.GetAuthenticationTokenAsync(user, "AuthApi", "ResetOTP");

            if (storedOtp != dto.Otp)
                throw new Exception("Invalid OTP");

            user.OtpExpiration = null;
            await _userManager.UpdateAsync(user);
            await _userManager.RemoveAuthenticationTokenAsync(user, "AuthApi", "ResetOTP");

            user.IsVerified = true;

            await _userManager.UpdateAsync(user);

            var accessToken = await _tokenService.CreateAccessToken(user);
            var refreshToken = await _tokenService.CreateRefreshToken(user);

            return new AuthResponseDto(
                accessToken,
                refreshToken.Token,
                user.Id.ToString(),
                user.FullName,
                user.Email,
                (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User"
            );
        }

        // =========================
        // RESET PASSWORD (AFTER OTP)
        // =========================
        public async Task<AuthResponseDto> ResetForgetPassword(ResetForgetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email)
                ?? throw new Exception("User not found");

            if (!user.IsVerified)
                throw new Exception("OTP not verified");

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, dto.NewPassword);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            user.IsVerified = false;

            await _userManager.UpdateAsync(user);

            var accessToken = await _tokenService.CreateAccessToken(user);
            var refreshToken = await _tokenService.CreateRefreshToken(user);

            return new AuthResponseDto(
                accessToken,
                refreshToken.Token,
                user.Id.ToString(),
                user.FullName,
                user.Email,
                (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User"
            );
        }

        // =========================
        // REFRESH TOKEN
        // =========================
        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            var storedToken = await _context.RefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token == dto.RefreshToken)
                ?? throw new Exception("Invalid refresh token");

            if (!storedToken.IsActive)
                throw new Exception("Token expired or revoked");

            storedToken.IsRevoked = true;

            await _context.SaveChangesAsync();

            var accessToken = await _tokenService.CreateAccessToken(storedToken.User);
            var refreshToken = await _tokenService.CreateRefreshToken(storedToken.User);

            return new AuthResponseDto(
                accessToken,
                refreshToken.Token,
                storedToken.User.Id.ToString(),
                storedToken.User.FullName,
                storedToken.User.Email,
                (await _userManager.GetRolesAsync(storedToken.User)).FirstOrDefault() ?? "User"
            );
        }

        // =========================
        // LOGOUT
        // =========================
        public async Task LogoutAsync(RefreshTokenDto dto)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == dto.RefreshToken)
                ?? throw new Exception("Invalid token");

            storedToken.IsRevoked = true;

            await _context.SaveChangesAsync();
        }

        // =========================
        // CHANGE PASSWORD
        // =========================
        public async Task ChangePasswordAsync(string userId, ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            var result = await _userManager.ChangePasswordAsync(
                user, dto.CurrentPassword, dto.NewPassword);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // =========================
        // OAUTH LOGIN
        // =========================
        public async Task<AuthResponseDto> HandleOAuthLoginAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    IsVerified = true
                };

                var createResult = await _userManager.CreateAsync(user);

                if (!createResult.Succeeded)
                    throw new Exception(string.Join(", ", createResult.Errors.Select(e => e.Description)));

                await _userManager.AddToRoleAsync(user, "User");

                user = await _userManager.FindByEmailAsync(email)
                    ?? throw new Exception("User not found");
            }

            var accessToken = await _tokenService.CreateAccessToken(user);
            var refreshToken = await _tokenService.CreateRefreshToken(user);

            return new AuthResponseDto(
                accessToken,
                refreshToken.Token,
                user.Id.ToString(),
                user.FullName,
                user.Email,
                (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User"
            );
        }

        // =========================
        // ASSIGN ROLE
        // =========================
        public async Task AssignRoleAsync(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new Exception("User not found");

            if (!await _roleManager.RoleExistsAsync(role))
                throw new Exception("Role does not exist");

            await _userManager.AddToRoleAsync(user, role);
        }
    }
}