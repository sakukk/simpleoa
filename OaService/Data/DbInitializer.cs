using Microsoft.AspNetCore.Identity;
using OaService.Models;

namespace OaService.Data;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Add Roles
        string[] roles = { "Staff", "Manager" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }

        // Add Manager
        var manager = new ApplicationUser
        {
            UserName = "manager@example.com",
            Email = "manager@example.com",
            EmailConfirmed = true,
            IsActive = true
        };

        if (await userManager.FindByEmailAsync(manager.Email) == null)
        {
            await userManager.CreateAsync(manager, "Manager123!");
            await userManager.AddToRoleAsync(manager, "Manager");
        }

        // Add Staff
        var staff = new ApplicationUser
        {
            UserName = "staff@example.com",
            Email = "staff@example.com",
            EmailConfirmed = true,
            IsActive = true,
            ManagerId = manager.Id.ToString()
        };

        if (await userManager.FindByEmailAsync(staff.Email) == null)
        {
            await userManager.CreateAsync(staff, "Staff123!");
            await userManager.AddToRoleAsync(staff, "Staff");
        }
    }
} 