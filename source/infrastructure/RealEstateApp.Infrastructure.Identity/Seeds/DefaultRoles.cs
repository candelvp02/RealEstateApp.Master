using Microsoft.AspNetCore.Identity;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace RealEstateApp.Infrastructure.Identity.Seeds;

public static class DefaultRoles
{
    public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
    {
        string[] roles = { "Client", "Agent", "Admin", "Developer" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = role });
            }
        }
    }
}
