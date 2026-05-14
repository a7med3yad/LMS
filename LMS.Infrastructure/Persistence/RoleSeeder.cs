using LMS.Domain.Common.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LMS.Infrastructure.Persistence;

/// <summary>
/// Seeds required Identity roles into AspNetRoles on startup.
/// Idempotent — safe to call on every application start.
/// </summary>
public static class RoleSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<RoleManager<IdentityRole<Guid>>>>();

        foreach (var roleName in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                if (result.Succeeded)
                {
                    logger.LogInformation("Seeded Identity role: {Role}", roleName);
                }
                else
                {
                    logger.LogError("Failed to seed role {Role}: {Errors}",
                        roleName,
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
