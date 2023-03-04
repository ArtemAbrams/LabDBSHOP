using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabOOP.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LabOOP.Controllers
{
    public class OrdersController : Controller
    {
        private readonly DBSHOPContext _context;

        public OrdersController(DBSHOPContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
           var dBSHOPContext = await _context.Orders.Include(o => o.Client).Include(o => o.Deliver).Include(o => o.ProductsOrders).ToListAsync();
           var dBProducts = await _context.ProductsOrders.ToListAsync();
           /* if (dBProducts.Count == 0)
                foreach (var item in dBSHOPContext)
                {
                    var elem = await _context.Orders.FirstOrDefaultAsync(o => o.Id == item.Id);
                    if(elem != null)
                    {
                        _context.Remove(elem);
                        await _context.SaveChangesAsync();
                    }
                }
            else*/
                foreach (var item in dBSHOPContext)
                {   
             
                        var countOfProducts = (from elem in dBProducts where elem.OrderId == item.Id select elem).Count();
                        if (item.DateOrder == null)
                        {
                           foreach(var orderproduct in item.ProductsOrders)
                             _context.Remove(orderproduct);
                           _context.Remove(item);
                           await _context.SaveChangesAsync();
                        }
                    
                }
           // await _context.SaveChangesAsync();
            var filteredOrders = await _context.Orders.Include(o => o.Client).Include(o => o.Deliver).ToListAsync();
            return View(filteredOrders);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPurchase(int orderId)
        {
            var productCount = await _context.ProductsOrders.Where(elem => elem.OrderId == orderId).CountAsync();
            if (productCount > 0)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(elem => elem.Id == orderId);
                if (order == null)
                    return NotFound();
                else
                {
                    order.DateOrder = DateTime.Now;
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(a => a.ProductsOrders)
                .Include(d => d.Deliver)
                .ThenInclude(b => b.Transport)
                .FirstOrDefaultAsync(m => m.Id == id);
            var productsInOrder = await _context.ProductsOrders.Where(elem => elem.OrderId == id).Include(o => o.Product).ThenInclude(b => b.Country).ToListAsync();
            if (order == null)
            {
                return NotFound();
            }
            var deliver = order.Deliver;
            ViewBag.Deliver = deliver;
            return View(productsInOrder);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id");
            ViewData["DeliverId"] = new SelectList(_context.Delivers.Include(o => o.Transport), "Id", "DisplayText");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,DateOrder,DeliverId,,Address")] Order order)
        {
            if (ModelState.IsValid)
            {
                string text = order.Address;
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("AddProduct", "Products", new { id = order.Id });
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", order.ClientId);
            ViewData["DeliverId"] = new SelectList(_context.Delivers.Include(o => o.Transport), "Id", "Name", order.DeliverId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", order.ClientId);
            ViewData["DeliverId"] = new SelectList(_context.Delivers.Include(o => o.Transport), "Id", "DisplayText", order.DeliverId);
            return View(order);
        }
        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,DateOrder,DeliverId,Address")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("AddProduct", "Products", new { id = id });
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", order.ClientId);
            ViewData["DeliverId"] = new SelectList(_context.Delivers.Include(o => o.Transport), "Id", "DisplayText", order.DeliverId);
            return View(order);
        }
        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Deliver)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'DBSHOPContext.Orders'  is null.");
            }
            var order = await _context.Orders
                .Include(o => o.ProductsOrders)
                .Include(o => o.Deliver).Include(o=>o.Feedbacks)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order != null)
            {
                foreach(var o in order.ProductsOrders)
                    _context.Remove(o);
                foreach (var o in order.Feedbacks)
                    _context.Remove(o);
                _context.Orders.Remove(order);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return _context.Orders.Any(e => e.Id == id);
        }
    }
}
