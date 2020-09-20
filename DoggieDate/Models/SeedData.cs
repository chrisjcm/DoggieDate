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
        private static UserManager<ApplicationUser> _userManager;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using ApplicationDbContext context = serviceProvider.GetService<ApplicationDbContext>();

            _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            // Seed roles if not in db
            if (!context.Roles.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                roleStore.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
                roleStore.CreateAsync(new IdentityRole { Name = "Member", NormalizedName = "MEMBER" });
            }

            // Seed users if not in db
            if (!context.User.Any())
            {

                string[] userNames = { "admin@doggie.se", "admin@doggie.com" };

                _ = CreateUserAsync(userNames[0], "Admin").Result;
                _ = CreateUserAsync(userNames[1], "Admin").Result;
            }

            // Test hitta users med rollen Admin
            //List<ApplicationUser> admins = _userManager.GetUsersInRoleAsync("Admin").Result.ToList();

            // Test hitta users med rollen Member
            //List<ApplicationUser> users = _userManager.GetUsersInRoleAsync("Member").Result.ToList();

            context.SaveChanges();
        }

        private static async Task<IdentityResult> CreateUserAsync(string userName, string role)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                NormalizedUserName = userName.ToUpper(),
                Email = userName,
                NormalizedEmail = userName.ToUpper(),
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(user, "Admin_123");

            return await AssignRoleAsync(user.Email, role);
        }

        /// <summary>
        /// Assign role to selected user
        /// </summary>
        public static async Task<IdentityResult> AssignRoleAsync(string email, string role)
        {
            // Find user with email to get ID
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            // Add user to role
            return await _userManager.AddToRoleAsync(user, role);
        }
    }
}
