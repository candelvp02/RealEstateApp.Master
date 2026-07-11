using Microsoft.AspNetCore.Identity;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace RealEstateApp.Infrastructure.Identity.Seeds;

public static class DefaultUsers
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
    {
        await CreateUserIfNotExistsAsync(userManager, new ApplicationUser
        {
            FirstName = "System",
            LastName = "Administrator",
            UserName = "admin",
            Email = "admin@realestateapp.com",
            Cedula = "00000000001",
            IsActive = true,
            EmailConfirmed = true
        }, "Admin123*", "Admin");

        await CreateUserIfNotExistsAsync(userManager, new ApplicationUser
        {
            FirstName = "Default",
            LastName = "Client",
            UserName = "client",
            Email = "client@realestateapp.com",
            Phone = "8091234567",
            IsActive = true,
            EmailConfirmed = true
        }, "Client123*", "Client");

        await CreateUserIfNotExistsAsync(userManager, new ApplicationUser
        {
            FirstName = "Default",
            LastName = "Agent",
            UserName = "agent",
            Email = "agent@realestateapp.com",
            Phone = "8097654321",
            IsActive = true,
            EmailConfirmed = true
        }, "Agent123*", "Agent");

        await CreateUserIfNotExistsAsync(userManager, new ApplicationUser
        {
            FirstName = "System",
            LastName = "Developer",
            UserName = "developer",
            Email = "developer@realestateapp.com",
            Cedula = "00000000002",
            IsActive = true,
            EmailConfirmed = true
        }, "Developer123*", "Developer");
    }

    private static async Task CreateUserIfNotExistsAsync(
        UserManager<ApplicationUser> userManager,
        ApplicationUser user,
        string password,
        string role)
    {
        var existing = await userManager.FindByNameAsync(user.UserName!);
        if (existing is null)
        {
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}