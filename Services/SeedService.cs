using Microsoft.AspNetCore.Identity;
using Star_Security.Models;

namespace Star_Security.Services
{
    public class SeedService
    {
        public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] roles = ["Admin", "Staff"];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string Email = "admin@gmail.com";
            string Password = "Admin@123";

            if (await userManager.FindByEmailAsync(Email) == null) 
            {
                var adminUser = new AppUser
                {
                    UserName = Email,
                    Email = Email,
                    FullName = "Hamza Aamir",
                    EmpCode = "ADM001",
                    EmailConfirmed = true,
                    Address = "Head Office",
                    ContactNumber = "0000000000",
                    EducationalQualification = "N/A",
                    CreatedAt = DateTime.UtcNow
                };

                var res = await userManager.CreateAsync(adminUser, Password);

                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
