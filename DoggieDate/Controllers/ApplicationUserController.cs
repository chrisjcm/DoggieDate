using DoggieDate.Data;
using DoggieDate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

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

		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.User;
			return View(await applicationDbContext.ToListAsync());
		}

		public IActionResult Profile(string id)
		{
			ApplicationUser user;

			if (string.IsNullOrWhiteSpace(id))
			{
				user = _userManager.GetUserAsync(User).Result;
			}
			else
			{
				user = _userManager.FindByIdAsync(id).Result;
			}

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

		[Authorize(Roles = "Admin")]
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


		[BindProperty]
		public InputModel Input { get; set; }

        public class InputModel
        {
            public string RecieverId { get; set; }
            public string Content { get; set; }
        }

        public IActionResult SendMessage(string id)
        {
			TempData["RecieverId"] = id;

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SendMessage(Message message)
		{
			message = new Message
			{
				ReceiverId = TempData["RecieverId"].ToString(),
				IsRead = false,
				SenderId = _userManager.GetUserAsync(User).Result.Id,
				Reported = false,
				TimeStamp = DateTime.Now,
				Content = Input.Content
			};

			_context.Add(message);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Profile));
		}
	}
}