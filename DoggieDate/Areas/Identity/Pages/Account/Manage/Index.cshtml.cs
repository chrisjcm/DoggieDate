using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DoggieDate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DoggieDate.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [Display(Name = "Användarnamn")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Avatar")]
            public string Avatar { get; set; }
            [Display(Name = "Hundnamn")]
			public string Dogname { get; set; }

			[Display(Name = "Ägare")]
			public string Owner { get; set; }

			//Landskap
			[Display(Name = "Landskap")]
			public string Region { get; set; }

			[Display(Name = "Ålder")]
			public int? Age { get; set; }

			[Display(Name = "Hundras")]
			public string Breed { get; set; }
		}

        private async Task LoadAsync(ApplicationUser user)
        {
            var applicationUser = await _userManager.FindByIdAsync(user.Id);

            Username = applicationUser.UserName;

            Input = new InputModel
            {
                Avatar = applicationUser.Avatar,
                Dogname = applicationUser.Dogname,
                Owner = applicationUser.Owner,
                Region = applicationUser.Region,
                Age = applicationUser.Age,
                Breed = applicationUser.Breed
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Något gick fel vid inläsning av användare: ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Något gick fel vid inläsning av användare: ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (!(Input is null))
            {
                user.Avatar = Input.Avatar;
                user.Dogname = Input.Dogname;
                user.Owner = Input.Owner;
                user.Region = Input.Region;
                user.Age = Input.Age;
                user.Breed = Input.Breed;

                await _userManager.UpdateAsync(user);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Din profil har uppdaterats";
            return RedirectToPage();
        }
    }
}
