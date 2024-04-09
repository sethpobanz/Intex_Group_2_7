using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityLab.Models;
using SecurityLab.Models.ViewModels;
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

            return View();
        }

        public IActionResult LegoList(string? legoType, int pageNum = 1)
        {
            int pageSize = 5;
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
