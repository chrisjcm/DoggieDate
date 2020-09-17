using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoggieDate.Data;
using DoggieDate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Contacts()
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;
            user.Contacts = _context.Contact.Include(c => c.User).Include(c => c.UserContact).Where(c => c.UserId == _userManager.GetUserId(HttpContext.User).ToString() && c.Accepted == true || c.ContactId == _userManager.GetUserId(HttpContext.User).ToString() && c.Accepted == true).ToList<Contact>();
            return View(user);
            
        }
        public async Task<IActionResult> DeleteContact(Contact id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .Include(c => c.User)
                .Include(c => c.UserContact)
                .FirstOrDefaultAsync(m => m.UserId == id.UserId);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Contact id)
        {

            _context.Contact.Remove(id);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Contacts));
        }




    }
}
