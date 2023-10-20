using Microsoft.AspNetCore.Identity;

namespace WiseProject.Models
{
    public class UserRoleInit
    {
        public static async Task InitAsync(IServiceProvider service)
        {
            var roleManager = service.GetRequiredService<RoleManager<Role>>();
            var userManager = service.GetRequiredService<UserManager<User>>();

            string[] roleNames = { "Admin", "User", "Instructor" };

            IdentityResult roleResult;
            foreach (var role in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    roleResult = await roleManager.CreateAsync(new Role(){Name = role});
                }
            }

            var email = "admin@admin.com";
            var pass = "Abcd!1234";

            if (userManager.FindByEmailAsync(email).Result == null)
            {
                User admin = new()
                {
                    Email = email,
                    UserName = email
                };
                IdentityResult result = await userManager.CreateAsync(admin, pass);
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                }
            }
        }
    }
}
