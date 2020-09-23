using DoggieDate.Data;
using DoggieDate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

		//public async Task<IActionResult> Index()
		//{
		//	var applicationDbContext = _context.User;
		//	return View(await applicationDbContext.ToListAsync());
		//}

		public IActionResult Profile(string id)
		{
			ApplicationUser user;
			ViewData["LoggedInUser"] = _userManager.GetUserAsync(User).Result;
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
			ApplicationUser currentUser = _userManager.GetUserAsync(User).Result;
			var a = _userManager.GetUserId(HttpContext.User).ToString();
			IEnumerable<ApplicationUser> users = _context.User.ToList().Where(c=>c.Id != currentUser.Id);



			HashSet<string> userList = new HashSet<string>(_context.Contact
				.Include(c => c.User)
				.Include(c => c.UserContact)
				.Where(c =>
							c.UserId == a && c.Accepted == true
						|| c.ContactId == a && c.Accepted == true).Select(x => x.UserId));

			HashSet<string> userList2 = new HashSet<string>(_context.Contact
				.Include(c => c.User)
				.Include(c => c.UserContact)
				.Where(c =>
							c.UserId == a && c.Accepted == true
						|| c.ContactId == a && c.Accepted == true).Select(x => x.ContactId));

			userList.UnionWith(userList2);

			users = users.Where(c => !userList.Contains(c.Id));
			return View(users);
		}

		public IActionResult Messages()
		{

			ApplicationUser user = _userManager.GetUserAsync(User).Result;

			return View(user);
		}

		//[Authorize(Roles = "Admin")]
		public IActionResult Contacts()
		{
			ApplicationUser user = _userManager.GetUserAsync(User).Result;


			var a = _userManager.GetUserId(HttpContext.User).ToString();
			user.Contacts = _context.Contact
				.Include(c => c.User)
				.Include(c => c.UserContact)
				.Where(c => 
							c.UserId == a && c.Accepted == true 
						|| c.ContactId == a && c.Accepted == true)
				.ToList<Contact>();
			
			
			return View(user);
		}

		public async Task<IActionResult> AddContact(string id)
        {
			
			ApplicationUser currentUser= _userManager.GetUserAsync(User).Result;		
			ApplicationUser ContactUser = _context.User.Find(id);
			var myContact =await _context.Contact.Where(c => c.UserId == id && c.ContactId == currentUser.Id || c.UserId == currentUser.Id && c.ContactId == id)
				.FirstOrDefaultAsync();
			

			if (myContact == null)
			{
				Contact cont = new Contact
				{
					UserId = currentUser.Id,
					ContactId = id,
					Accepted = false,
					Blocked = false


				};
			_context.Add(cont);
			}
			else if (myContact.Accepted)
            {
				myContact.Accepted = false;
				_context.Contact.Update(myContact);
			}
			else if (!myContact.Accepted && myContact.UserId == currentUser.Id)
			{
				
				_context.Contact.Remove(myContact);
			}
			else
            {
				myContact.Accepted = true;
				_context.Contact.Update(myContact);
            }
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(SearchUsers));

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