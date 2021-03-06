﻿using DoggieDate.Data;
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

                string[] userNames = { 
                        "admin@doggie.se", 
                        "admin@doggie.com", 
                        "adam@hej.com", 
                        "eva@hej.com", 
                        "kalle@hej.com",
                        "martin@hej.com", 
                        "johan@hej.com", 
                        "sven@hej.com" 
                    };

                _ = CreateUserAsync(userNames[0], "Admin").Result;
                _ = CreateUserAsync(userNames[1], "Admin").Result;
                _ = CreateUserAsync(userNames[2], "Member").Result;
                _ = CreateUserAsync(userNames[3], "Member").Result;
                _ = CreateUserAsync(userNames[4], "Member").Result;
                _ = CreateUserAsync(userNames[5], "Member").Result;
                _ = CreateUserAsync(userNames[6], "Member").Result;
                _ = CreateUserAsync(userNames[7], "Member").Result;




                //Better seeds needed.
				//var user1 = new ApplicationUser
				//{
				//	UserName = userName,
				//	NormalizedUserName = userName.ToUpper(),
				//	Email = userName,
				//	NormalizedEmail = userName.ToUpper(),
				//	EmailConfirmed = true,
				//	Avatar = "https://www.iconfinder.com/data/icons/dog-and-cat-3/64/08-golden_retriever-puppy-pets-avatar-animals-animal-dog-512.png",
				//	Owner = userName.Substring(0, userName.IndexOf('@')),
				//	Breed = "Okänd",
				//	Region = "Sverige",
				//	Dogname = "Namnlös",
				//	Age = 0

				//};
				//_ = CreateUserAsync(user1, "Member").Result;

				//var user2 = new ApplicationUser
				//{
				//	UserName = userName,
				//	NormalizedUserName = userName.ToUpper(),
				//	Email = userName,
				//	NormalizedEmail = userName.ToUpper(),
				//	EmailConfirmed = true,
				//	Avatar = "https://www.iconfinder.com/data/icons/dog-and-cat-3/64/08-golden_retriever-puppy-pets-avatar-animals-animal-dog-512.png",
				//	Owner = userName.Substring(0, userName.IndexOf('@')),
				//	Breed = "Okänd",
				//	Region = "Sverige",
				//	Dogname = "Namnlös",
				//	Age = 0

				//};


				//var user3 = new ApplicationUser
				//{
				//	UserName = userName,
				//	NormalizedUserName = userName.ToUpper(),
				//	Email = userName,
				//	NormalizedEmail = userName.ToUpper(),
				//	EmailConfirmed = true,
				//	Avatar = "https://www.iconfinder.com/data/icons/dog-and-cat-3/64/08-golden_retriever-puppy-pets-avatar-animals-animal-dog-512.png",
				//	Owner = userName.Substring(0, userName.IndexOf('@')),
				//	Breed = "Okänd",
				//	Region = "Sverige",
				//	Dogname = "Namnlös",
				//	Age = 0

				//};
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
                EmailConfirmed = true,
                Avatar = "https://www.iconfinder.com/data/icons/dog-and-cat-3/64/08-golden_retriever-puppy-pets-avatar-animals-animal-dog-512.png",
                Owner = userName.Substring(0, userName.IndexOf('@')),
                Breed = "Okänd",
                Region = "Sverige",
                Dogname = "Namnlös",
                Age = 0
               
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
