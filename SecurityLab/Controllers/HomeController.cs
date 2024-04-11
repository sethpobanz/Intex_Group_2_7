using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using SecurityLab.Models;
using SecurityLab.Models.ViewModels;
using System.Diagnostics;

namespace SecurityLab.Controllers
{
    public class HomeController : Controller
    {
        private IProductInterface _repo;
        private IUserRecInterface _recRepo;

        public HomeController(IProductInterface repo, IUserRecInterface recRepo)
        {
            _repo = repo;
            _recRepo = recRepo;
        }

        public IActionResult Index()
        {
            // Get all UserPipelines from _recRepo
            var userPipelines = _recRepo.UserPipelines.ToList();

            // Extract the list of ProductIds from UserPipelines
            var productIds = userPipelines.Select(p => p.ProductId).ToList();

            // Filter products from _repo that match the ProductIds in userPipelines
            var rec = _repo.Products.Where(p => productIds.Contains(p.ProductId)).ToList();

            return View(rec);
        }

        public IActionResult LegoList(string? legoType, int pageNum = 1)
        {
            int defaultPageSize = 5;  // Default page size
            int pageSize = defaultPageSize;

            // Check if itemsPerPage is provided in the query string
            if (Request.Query.TryGetValue("PaginationInfo.itemsPerPage", out var itemsPerPageValue))
            {
                if (int.TryParse(itemsPerPageValue, out int parsedItemsPerPage))
                {
                    pageSize = parsedItemsPerPage;
                }
            }

            var blah = new ProductsListViewModel
            {
                Products = _repo.Products
                    .Where(x => x.Category1 == legoType || legoType == null)
                    .OrderBy(x => x.Name)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize),
                PaginationInfo = new PaginationInfo
                {
                    currentPage = pageNum,
                    itemsPerPage = pageSize,
                    totalItems = legoType == null ? _repo.Products.Count() : _repo.Products.Where(x => x.Category1 == legoType).Count()
                },
                CurrentProductType = legoType
            };

            return View(blah);
        }



        [Authorize(Roles = "Admin")]
        public IActionResult AdminPortal()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult LegoSingle(int productId)
        {
            // Find the product by productId
            var product = _repo.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                // Product not found, handle error or redirect to another page
                return NotFound(); // Return a 404 Not Found error
            }

            // Pass the product details to the view
            return View(product); // Assuming you have a LegoSingle.cshtml view to display product details
        }      



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
