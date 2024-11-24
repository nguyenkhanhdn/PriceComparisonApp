using Microsoft.AspNetCore.Mvc;
using PriceComparisonApp.Data;
using PriceComparisonApp.Models;
using System.Diagnostics;

namespace PriceComparisonApp.Controllers
{
    public class HomeController : Controller
    {


        private readonly PriceComparisonDbContext _context;
        public HomeController(PriceComparisonDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult Search(string product)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'PriceComparisonDbContext.Product'  is null.");
            }

            var products = from m in _context.Products
                         select m;

            if (!String.IsNullOrEmpty(product))
            {
                products = products.Where(s => s.Name!.ToUpper().Contains(product.ToUpper()));
            }

            return View("Products",products.ToList());

        }

        public IActionResult ProductsByCategory(string category)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'PriceComparisonDbContext.Product'  is null.");
            }

            var products = from m in _context.Products
                           select m;

            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(s => s.Category!.ToUpper().Contains(category.ToUpper()));
            }

            return View("Products", products.ToList());

        }


        public IActionResult Index()
        {

            var products = _context.Products.OrderByDescending(p=>p.Name).Take(3).ToList();
            var minPrice = products.MinBy(p => p.Price);
            ViewBag.MinPrice = minPrice.Price;
            return View(products);
        }


        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
