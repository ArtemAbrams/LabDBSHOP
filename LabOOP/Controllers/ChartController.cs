using LabOOP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace LabOOP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DBSHOPContext _context;

        public ChartController(DBSHOPContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
           var allProducts = _context.Products.Include(a => a.ProductsOrders).ToList();
           List<object> productsCount = new List<object>();
            productsCount.Add(new object[] { "Name", "Count", "Price", "Total price" });
           foreach (var product in allProducts) 
            {
              var count = (from item in product.ProductsOrders select item.Count).Sum();             
              productsCount.Add(new object [] {product.Name, count,  product.Price, (product.Price * count)});
            }
           return new JsonResult(productsCount);
        }

        [HttpGet("ProductList/{orderId}")]
        public JsonResult ProductList(int? orderId)
        {
            var allProducts = _context.ProductsOrders
             .Where(po => po.OrderId == orderId)
             .Join(_context.Products, po => po.ProductId, p => p.Id, (po, p) => new { p.Name, p.Price, po.Count })
             .ToList();
            List<object> productsCount = new List<object>();
            productsCount.Add(new object[] { "Назва продуктів", "Загальна ціна у замовленні" });
            foreach (var product in allProducts)
            {
                productsCount.Add(new object[] { product.Name, (product.Price*product.Count) });
            }
            
            return new JsonResult(productsCount);
        }
    }
}
