using GraduationProject.Models;
using Microsoft.AspNetCore.Identity;

namespace GraduationProject
{
    public class DataSeeder
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "secretary", "professor", "student" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            await CreateUser(userManager, "secretarInfo@uab.ro", "Uab1234!", "secretary");
            await CreateUser(userManager, "incze.arpad@uab.ro", "Uab1234!", "professor");
            await CreateUser(userManager, "muntean.maria@uab.ro", "Uab1234!", "professor");
            await CreateUser(userManager, "barbut.dragos.pabd23@uab.ro", "Uab1234!", "student");
            await CreateUser(userManager, "dragan.andrei.info24@uab.ro", "Uab1234!", "student");
        }

        private static async Task CreateUser(UserManager<ApplicationUser> userManager, string email, string password, string role)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new ApplicationUser { UserName = email, Email = email };
                var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, role);
            }
        }
    }

}
