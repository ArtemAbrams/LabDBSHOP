using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabOOP.Models;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LabOOP.Controllers
{
    public class DeliversController : Controller
    {
        private readonly DBSHOPContext _context;

       public DeliversController(DBSHOPContext context)
        {
            _context = context;
        }

        // GET: Delivers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Delivers.Include(o=>o.Transport).ToListAsync());
        }

        // GET: Delivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Delivers == null)
            {
                return NotFound();
            }

            var deliver = await _context.Delivers.Include(o => o.Transport)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deliver == null)
            {
                return NotFound();
            }

            return View(deliver);
        }

        // GET: Delivers/Create
        public IActionResult Create()
        {
            ViewData["TransportId"] = new SelectList(_context.Transports, "Id", "Name");
            return View();
        }

        // POST: Delivers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PhoneNumber,TransportId,Name,Surname")] Deliver deliver)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deliver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TransportId"] = new SelectList(_context.Transports, "Id", "Name");
            return View(deliver);
        }

        // GET: Delivers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Delivers == null)
            {
                return NotFound();
            }

            var deliver = await _context.Delivers.FindAsync(id);
            if (deliver == null)
            {
                return NotFound();
            }
            ViewData["TransportId"] = new SelectList(_context.Transports, "Id", "Name", deliver.TransportId);
            return View(deliver);
        }

        // POST: Delivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PhoneNumber,TransportId,Name,Surname")] Deliver deliver)
        {
            if (id != deliver.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliverExists(deliver.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TransportId"] = new SelectList(_context.Transports, "Id", "Name", deliver.TransportId);
            return View(deliver);
        }
        // GET: Delivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Delivers == null)
            {
                return NotFound();
            }

            var deliver = await _context.Delivers.Include(o => o.Orders).Include(o => o.Transport)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deliver == null)
            {
                return NotFound();
            }
            var list = from elem in _context.ProductsOrders.Where(o => o.Id == id).ToList() select elem.OrderId ;
            ViewBag.CountOfOrder = deliver.Orders.Count;
            ViewBag.OrderData = deliver.Orders.ToList();
            return View(deliver);
        }

        // POST: Delivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Delivers == null)
            {
                return Problem("Entity set 'DBSHOPContext.Delivers'  is null.");
            }
            var deliver = await _context.Delivers.Include(o=>o.Orders).ThenInclude(o=>o.Feedbacks).FirstOrDefaultAsync(elem => elem.Id == id);
            if (deliver != null)
            {
                foreach(var order in deliver.Orders)
                {
                    foreach (var feedback in order.Feedbacks)
                    {
                        _context.Remove(feedback);
                        await _context.SaveChangesAsync();
                    }
                }
                deliver = await _context.Delivers.Include(o => o.Orders).ThenInclude(o => o.ProductsOrders).FirstOrDefaultAsync(elem => elem.Id == id);
                if (deliver != null) 
                {
                    foreach (var order in deliver.Orders)
                    {
                        foreach (var productOrders in order.ProductsOrders)
                        {
                            _context.Remove(productOrders);
                            await _context.SaveChangesAsync();
                        }
                        _context.Remove(order);
                        await _context.SaveChangesAsync();
                    }
                    _context.Delivers.Remove(deliver);
                }
                _context.Delivers.Remove(deliver);
            }        
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliverExists(int id)
        {
          return _context.Delivers.Any(e => e.Id == id);
        }
    }
}
