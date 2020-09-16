using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoggieDate.Data;
using DoggieDate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoggieDate.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public ApplicationUserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;
            return View(user);
        }

        //[AllowAnonymous]
        public IActionResult SearchUsers()
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;

            return View(user);
        }

        public IActionResult Messages()
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;

            return View(user);
        }
    }
}
