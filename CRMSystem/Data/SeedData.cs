using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Rollen erstellen
            string[] rollenNamen = { "Administrator", "Manager", "Mitarbeiter" };
            foreach (var rollenName in rollenNamen)
            {
                var roleExist = await roleManager.RoleExistsAsync(rollenName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(rollenName));
                }
            }

            // Admin-Benutzer erstellen
            var adminUser = await userManager.FindByEmailAsync("admin@crm.de");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@crm.de",
                    Email = "admin@crm.de",
                    Vorname = "Admin",
                    Nachname = "User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }
        }
    }
}