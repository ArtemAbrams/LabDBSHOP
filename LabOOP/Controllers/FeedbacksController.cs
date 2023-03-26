using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabOOP.Models;
using LabOOP.ViewModels;

namespace LabOOP.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly DBSHOPContext _context;

        public FeedbacksController(DBSHOPContext context)
        {
            _context = context;
        }

        // GET: Feedbacks
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
                return NotFound();
            ViewBag.orderId = id;
            var dBSHOPContext = _context.Feedbacks.
                Include(f => f.Order).
                Where(e => e.OrderId == id);
            return View(await dBSHOPContext.ToListAsync());
        }

        // GET: Feedbacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .Include(f => f.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.orderId = feedback?.OrderId ?? null;
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // GET: Feedbacks/Create
        public IActionResult Create(int? orderId)
        {
            if (orderId == null)
            {
                return NotFound();
            }
            ViewBag.orderId = orderId;
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? orderId, [Bind("Id,DateOfPublication,OrderId,Description")] Feedback feedback)
        {
            if(orderId == null) { return NotFound(); };
            if (ModelState.IsValid)
            {
                feedback.OrderId = orderId;
                feedback.DateOfPublication = DateTime.Now;
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {id = orderId});
            }
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .Include(f => f.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Feedbacks == null)
            {
                return Problem("Entity set 'DBSHOPContext.Feedbacks'  is null.");
            }
            var feedback = await _context.Feedbacks.FindAsync(id);
            int? orderId = null;
            if (feedback != null)
            {
                 orderId = feedback.OrderId;
                _context.Feedbacks.Remove(feedback);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {id = orderId});
        }

        private bool FeedbackExists(int id)
        {
          return _context.Feedbacks.Any(e => e.Id == id);
        }
    }
}
