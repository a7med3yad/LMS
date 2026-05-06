using LMS.Domain.Models;
using LMS.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LMS.Infrastructure.Services
{
    public class TokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public TokenService(
            IConfiguration configuration,
            AppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
        }

        // =============================
        // 🔐 ACCESS TOKEN
        // =============================
        public async Task<string> CreateAccessToken(ApplicationUser user)
        {
            var keyString = _configuration["Jwt:Key"]
                ?? throw new Exception("JWT Key is missing");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(keyString));

            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? "")
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // =============================
        // 🔁 CREATE REFRESH TOKEN
        // =============================
        public async Task<RefreshToken> CreateRefreshToken(ApplicationUser user)
        {
            var refreshToken = new RefreshToken
            {
                Token = GenerateSecureToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false,
                UserId = user.Id
            };

            var activeTokens = _context.RefreshTokens
                .Where(t => t.UserId == user.Id && !t.IsRevoked);

            foreach (var old in activeTokens)
            {
                old.IsRevoked = true;
                old.ReplacedByToken = refreshToken.Token;
            }

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        // =============================
        // 🔄 ROTATE REFRESH TOKEN
        // =============================
        public async Task<(string accessToken, RefreshToken refreshToken)> RefreshAsync(string token)
        {
            var existingToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token);

            if (existingToken == null)
                throw new Exception("Invalid token");

            if (!existingToken.IsActive)
                throw new Exception("Token is not active");

            var user = await _userManager.FindByIdAsync(existingToken.UserId.ToString());
            if (user == null)
                throw new Exception("User not found");

            // revoke old token
            existingToken.IsRevoked = true;

            // create new refresh token
            var newRefreshToken = new RefreshToken
            {
                Token = GenerateSecureToken(),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            existingToken.ReplacedByToken = newRefreshToken.Token;

            await _context.RefreshTokens.AddAsync(newRefreshToken);

            var newAccessToken = await CreateAccessToken(user);

            await _context.SaveChangesAsync();

            return (newAccessToken, newRefreshToken);
        }

        // =============================
        // 🚫 REVOKE TOKEN
        // =============================
        public async Task RevokeToken(string token)
        {
            var existingToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token);

            if (existingToken == null)
                throw new Exception("Token not found");

            existingToken.IsRevoked = true;

            await _context.SaveChangesAsync();
        }

        // =============================
        // 🔐 SECURE TOKEN GENERATOR
        // =============================
        private string GenerateSecureToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}