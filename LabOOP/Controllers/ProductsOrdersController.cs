using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabOOP.Models;

namespace LabOOP.Controllers
{
    public class ProductsOrdersController : Controller
    {
        private readonly DBSHOPContext _context;

        public ProductsOrdersController(DBSHOPContext context)
        {
            _context = context;
        }

        // GET: ProductsOrders
        public async Task<IActionResult> Index()
        {
            var dBSHOPContext = _context.ProductsOrders.Include(p => p.Order).Include(p => p.Product);
            return View(await dBSHOPContext.ToListAsync());
        }

        // GET: ProductsOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductsOrders == null)
            {
                return NotFound();
            }

            var productsOrder = await _context.ProductsOrders
                .Include(p => p.Order)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productsOrder == null)
            {
                return NotFound();
            }

            return View(productsOrder);
        }

        // GET: ProductsOrders/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Address");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description");
            return View();
        }

        // POST: ProductsOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderId,ProductId,Count")] ProductsOrder productsOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productsOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Address", productsOrder.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", productsOrder.ProductId);
            return View(productsOrder);
        }

        // GET: ProductsOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductsOrders == null)
            {
                return NotFound();
            }

            var productsOrder = await _context.ProductsOrders.FindAsync(id);
            if (productsOrder == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Address", productsOrder.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", productsOrder.ProductId);
            return View(productsOrder);
        }

        // POST: ProductsOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,ProductId,Count")] ProductsOrder productsOrder)
        {
            if (id != productsOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productsOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsOrderExists(productsOrder.Id))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Address", productsOrder.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", productsOrder.ProductId);
            return View(productsOrder);
        }

        // GET: ProductsOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductsOrders == null)
            {
                return NotFound();
            }

            var productsOrder = await _context.ProductsOrders
                .Include(p => p.Order)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productsOrder == null)
            {
                return NotFound();
            }

            return View(productsOrder);
        }

        // POST: ProductsOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductsOrders == null)
            {
                return Problem("Entity set 'DBSHOPContext.ProductsOrders'  is null.");
            }
            var productsOrder = await _context.ProductsOrders.FindAsync(id);
            if (productsOrder != null)
            {
                _context.ProductsOrders.Remove(productsOrder);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsOrderExists(int id)
        {
          return _context.ProductsOrders.Any(e => e.Id == id);
        }
    }
}
