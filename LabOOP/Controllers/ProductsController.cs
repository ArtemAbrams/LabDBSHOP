using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabOOP.Models;
using LabOOP.ViewModels;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace LabOOP.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DBSHOPContext _context;
        private int InitialAmount = 1;
        public ProductsController(DBSHOPContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var dBSHOPContext = _context.Products.Include(p => p.Country);
            return View(await dBSHOPContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            return View();
        }

        public async Task<IActionResult> AddProduct(int? id)
        {
            if (id == null || _context.Products ==  null)
            {
                return NotFound();             
            }
            var products = await _context.Products.ToListAsync();
            ViewBag.OrderId = id;
            return View(products);
        }
        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,WeightInKilograms,CountryId")] Product product)
        {
            if (ModelState.IsValid)
            {
        
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", product.CountryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", product.CountryId);
            return View(product);
        }
        public async Task<IActionResult> Buy(int? productId, int? orderId)
        {
            if(orderId == null || productId== null) 
            {
                return NotFound();
            }
            var product = await _context.Products
                .Include(p => p.Country)
                .FirstOrDefaultAsync(m => m.Id == productId);
            if(product == null) 
            {
              return NotFound();
            }
            ViewBag.OrderId = orderId;
            return View(product);

        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,WeightInKilograms,CountryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", product.CountryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'DBSHOPContext.Products'  is null.");
            }
            var product = await _context.Products.
                Include(o => o.ProductsOrders).
                FirstOrDefaultAsync(elem => elem.Id == id);
            if (product != null)
            {
                var orderIdlist = product.
                    ProductsOrders
                    .Select(o => o.OrderId)
                    .ToList();
                foreach(var Id in orderIdlist)
                {

                    DeleteOrders(Id);
                }
                _context.Products.Remove(product);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public void DeleteOrders(int? id)
        {
            var order =  _context.Orders.Include(o => o.Feedbacks).Include(a => a.ProductsOrders).FirstOrDefault(elem => elem.Id == id);
            if (order != null)
            {
                foreach (var feed in order.Feedbacks)
                    _context.Remove(feed);
                foreach (var ord in order.ProductsOrders)
                    _context.Remove(ord);
                _context.Remove(order);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmBuy(int orderId, int productId)
        {
            var OrderProduct = _context.ProductsOrders.Where(elem => elem.OrderId == orderId).Where(elem => elem.ProductId == productId).FirstOrDefault();
            if (OrderProduct != null)
            {
                OrderProduct.Count++;
                _context.Update(OrderProduct);
                await _context.SaveChangesAsync();
            }
            else
            {
                var orderProduct = new ProductsOrder
                {
                    OrderId = orderId,
                    ProductId = productId,
                    Count = InitialAmount,
                };
                _context.ProductsOrders.Add(orderProduct);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("AddProduct", "Products", new { id = orderId });
        }
        public async Task<IActionResult> DeleteProductOrder(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var product = await _context.ProductsOrders.FirstOrDefaultAsync(element => element.Id == id);
            if (product == null) 
            {
                return NotFound();
            }
            ViewBag.orderId = product.OrderId;
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProduct(int id, int count)
        {
            var orderProduct = await _context.ProductsOrders.FirstOrDefaultAsync(element => element.Id == id);
            if(orderProduct == null) { return NotFound(); }
            orderProduct.Count -= count;
            int? orderId = orderProduct.OrderId; 
            if (orderProduct.Count == 0)
                _context.Remove(orderProduct);
            else
                _context.Update(orderProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("ListOfProducts", "Products", new { id = orderId });

        }
        [HttpPost]
        public async Task<IActionResult> ConfirmPurchase(int orderId)
        {
            var productCount = await _context.ProductsOrders.
                Where(elem => elem.OrderId == orderId).CountAsync();
            if (productCount > 0) 
            { 
               var order = await _context.Orders.FirstOrDefaultAsync(elem  => elem.Id == orderId);
                if(order == null ) { return NotFound(); }
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(order.Id))
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
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ListOfProducts(int? id )
        {
            if (id == null)
            {
                return NotFound();
            }
            var basket = await _context.ProductsOrders.Include(product => product.Product).ThenInclude(b => b.Country).Where(item => item.OrderId== id).ToListAsync();
            if(basket == null)
            {
                return NotFound();
            }
            
            ViewBag.orderId = id;
            return View(basket);
        }
        private bool ProductExists(int id)
        {
          return _context.Products.Any(e => e.Id == id);
        }
    }
}
