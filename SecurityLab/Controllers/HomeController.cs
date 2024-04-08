using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityLab.Models;
using System.Diagnostics;

namespace SecurityLab.Controllers
{
    public class HomeController : Controller
    {
        private IProductInterface _repo;

        public HomeController(IProductInterface repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            ViewBag.ProductInfo = _repo.Products.FirstOrDefault(x => x.Name == "Harry Potter Classic Kit");

            // Check if ViewBag.ProductInfo is null or not found
            if (ViewBag.ProductInfo == null)
            {
                // Handle scenario where product is not found
                // For example, return a different view or show an error message
                return NotFound(); // or return a specific view
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secrets()
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
