using Microsoft.AspNetCore.Mvc;
using PriceComparisonApp.Data;
using PriceComparisonApp.Services;

namespace PriceComparisonApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController()
        {
            _productService = new ProductService();
        }

        public IActionResult Recommendations(int id)
        {
            var products = _productService.GetRecommendedProducts(id);
            var allProducts = _productService.GetRecommendedProducts(id);

            //var selectedProduct = allProducts.FirstOrDefault(p => p.Id == id);
            var selectedProduct = allProducts.FirstOrDefault();
            if (selectedProduct == null) return View(new RecommendationViewModel());

            var viewModel = new RecommendationViewModel
            {
                SelectedProduct = selectedProduct,
                RecommendedProducts = products
            };

            return View(viewModel);
        }
    }
}
