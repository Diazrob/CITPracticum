
using CITPracticum.Data;
using CITPracticum.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace CIT_Practicum.Data
{
    public class Seed
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.Student))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Student));
                if (!await roleManager.RoleExistsAsync(UserRoles.Employer))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Employer));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "robee_lou.diaz@lethbridgecollege.ca";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "Admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string studentUserEmail = "watnada@lethbridgecollege.ca";

                var studentUser = await userManager.FindByEmailAsync(studentUserEmail);
                if (studentUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "Student",
                        Email = studentUserEmail,
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.Student);
                }

                string employerUserEmail = "irah.loreto@lethbridgecollege.ca";

                var employerUser = await userManager.FindByEmailAsync(employerUserEmail);
                if (employerUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "Employer",
                        Email = employerUserEmail,
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.Employer);
                }
            }
        }
    }
}