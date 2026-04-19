using LMS.Api.DTOs.Auth;
using LMS.Api.DTOs.Auth;
using LMS.DTOs.Auth;
using LMS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LMS.Infrastructure.Services
{

    public class OAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenService _tokenService;
        public OAuthService(UserManager<ApplicationUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> HandleOAuthLogin(AuthenticateResult result)
        {
            var email = result.Principal?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email not provided by OAuth provider");
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    IsVerified = true
                };
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, "User");
            }
            return new AuthResponseDto(
                AccessToken: await _tokenService.CreateAccessToken(user),
                RefreshToken: (await _tokenService.CreateRefreshToken(user)).Token,
                UserId: user.Id.ToString(),
                FullName: user.FullName,
                Email: email,
                Role: user.Role.ToString()
            );

        }
    }
}
