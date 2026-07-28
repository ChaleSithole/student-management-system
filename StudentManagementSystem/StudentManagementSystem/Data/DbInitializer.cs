using Microsoft.AspNetCore.Identity;

namespace StudentManagementSystem.Data
{
    public static class DbInitializer
    {
        public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles =
            {
                "Administrator",
                "Lecturer"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string adminEmail = "admin@university.co.za";
            string adminPassword = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }

            string lecturerEmail = "lecturer@university.co.za";
            string lecturerPassword = "Lecturer@123";

            var lecturerUser = await userManager.FindByEmailAsync(lecturerEmail);

            if (lecturerUser == null)
            {
                lecturerUser = new IdentityUser
                {
                    UserName = lecturerEmail,
                    Email = lecturerEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(
                    lecturerUser,
                    lecturerPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(
                        lecturerUser,
                        "Lecturer");
                }
            }
        }
    }
}