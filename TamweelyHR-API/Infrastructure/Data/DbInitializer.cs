using Core.Helpers;
using Core.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
    
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));

            if (!await roleManager.RoleExistsAsync(Roles.User))
                await roleManager.CreateAsync(new IdentityRole(Roles.User));

       
            var adminEmail = "admin@admin.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser is null)
            {
                adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                var created = await userManager.CreateAsync(adminUser, "Admin123!");
                if (created.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, Roles.Admin);
            }

      
            var userEmail = "user@tuser.com";
            var normalUser = await userManager.FindByEmailAsync(userEmail);

            if (normalUser is null)
            {
                normalUser = new AppUser
                {
                    UserName = userEmail,
                    Email = userEmail
                };

                var created = await userManager.CreateAsync(normalUser, "User123!");
                if (created.Succeeded)
                    await userManager.AddToRoleAsync(normalUser, Roles.User);
            }
        }
    }
}
