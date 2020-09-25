using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoggieDate.Data;
using DoggieDate.Models;
using Microsoft.AspNetCore.Identity;

namespace DoggieDate.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public MessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Messages
        public async Task<IActionResult> Index()
        {
            ApplicationUser user =await _userManager.GetUserAsync(User);

            user.Messages = await _context.Message
                .Include(m => m.Receiver)
                .Include(m => m.Sender)
                .Where(c => c.ReceiverId == user.Id).ToListAsync();
            return View(user);

        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Receiver)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (message == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);

            if (message.ReceiverId == user.Id && !message.IsRead)
            {
                message.IsRead = true;
                _context.Message.Update(message);
                await _context.SaveChangesAsync();
            }

            return View(message);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            // endast kontakter
            
            ViewData["Receiver"] = new SelectList(_context.User, "Email", "Email");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReceiverId,Content")] Message message)
        {
            ApplicationUser uID = await _userManager.FindByEmailAsync(message.ReceiverId);
            message.ReceiverId = uID.Id;
            message.IsRead = false;
            message.SenderId = _userManager.GetUserAsync(User).Result.Id;
            message.Reported = false;
            message.TimeStamp = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Receiver"] = new SelectList(_context.User, "Email", "Email", message.ReceiverId);
            
            return View(message);
        }

        // GET: Messages/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var message = await _context.Message.FindAsync(id);
        //    if (message == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["ReceiverId"] = new SelectList(_context.User, "Id", "Id", message.ReceiverId);
        //    ViewData["SenderId"] = new SelectList(_context.User, "Id", "Id", message.SenderId);
        //    return View(message);
        //}

        //// POST: Messages/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SenderId,ReceiverId,Content,TimeStamp,IsRead,Reported")] Message message)
        //{
        //    if (id != message.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(message);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MessageExists(message.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ReceiverId"] = new SelectList(_context.User, "Id", "Id", message.ReceiverId);
        //    ViewData["SenderId"] = new SelectList(_context.User, "Id", "Id", message.SenderId);
        //    return View(message);
        //}

        //GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Receiver)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Message.FindAsync(id);
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private bool MessageExists(int id)
        //{
        //    return _context.Message.Any(e => e.Id == id);
        //}
    }
}
