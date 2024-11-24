using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
