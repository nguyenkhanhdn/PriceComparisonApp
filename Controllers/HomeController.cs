using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult ProdDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.Find(id);
            
            if (product == null)
            {
                return NotFound();
            }

            var twoRelatedProducts = _context.Products.
                Where(p => p.Name.Contains(product.Name)).Where(pp => pp.Id != id).ToList();
            ViewData["twoProducts"] = twoRelatedProducts;
            return View(product);
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
            try
            {
                var products = _context.Products.OrderByDescending(p => p.Name).Take(3).ToList();
                var minPrice = products.MinBy(p => p.Price);
                if (minPrice!= null)
                    ViewBag.MinPrice = minPrice.Price;
                
                return View(products);
            }
            catch (Exception ex)
            {

                return View();
            }

            
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
