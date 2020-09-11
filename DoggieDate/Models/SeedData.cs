using DoggieDate.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoggieDate.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>());

            if (!context.Roles.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                roleStore.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
                roleStore.CreateAsync(new IdentityRole { Name = "User", NormalizedName = "USER" });
            }

            //if (!context.User.Any())
            //{

            //    var user = new ApplicationUser
            //    {
            //        UserName = "TopDog",
            //        NormalizedUserName = "TOPDOG",
            //        Email = "admin@doggie.se",
            //        NormalizedEmail = "ADMIN@DOGGIE.SE",
            //        EmailConfirmed = true
            //    };

            //    var password = new PasswordHasher<ApplicationUser>();
            //    var hash = password.HashPassword(user, "Admin_123");
            //    user.PasswordHash = hash;

            //    var userStore = new UserStore<ApplicationUser>(context);
            //    userStore.CreateAsync(user);

            //    //UserManager<ApplicationUser> userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            //    //var result = userManager.CreateAsync(user, "Admin_123");
            //    //userManager.AddToRoleAsync(user, "Admin");

            //}

            //AssignRoles(serviceProvider, "admin@doggie.se");

            //context.SaveChangesAsync();
        }

        //public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email)
        //{
        //    UserManager<ApplicationUser> _userManager = services.GetService<UserManager<ApplicationUser>>();
        //    ApplicationUser user = await _userManager.FindByEmailAsync(email);
        //    var result = await _userManager.AddToRoleAsync(user, "Admin");

        //    return result;
        //}
    }
}
