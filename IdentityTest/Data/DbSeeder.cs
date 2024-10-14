using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityTest.Models;

namespace IdentityTest.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        // Ensure the database is created
        await context.Database.EnsureCreatedAsync();

        // Seed Roles
        string[] roleNames = { "Admin", "Writer" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Seed Admin User
        string adminEmail = "admin@example.com";
        string adminPassword = "AdminPass123!";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // Seed some initial jokes
        if (!await context.Jokes.AnyAsync())
        {
            context.Jokes.AddRange(
                new Joke { JokeText = "Why don't scientists trust atoms? Because they make up everything!", Author = "System" },
                new Joke { JokeText = "Why did the scarecrow win an award? He was outstanding in his field!", Author = "System" }
            );
            await context.SaveChangesAsync();
        }
    }
}