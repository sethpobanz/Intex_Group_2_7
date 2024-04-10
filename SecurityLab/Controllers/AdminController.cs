using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecurityLab.Models;
using SecurityLab.Models.ViewModels;
using static NuGet.Packaging.PackagingConstants;

namespace SecurityLab.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IProductInterface _prodRepo;
        private IOrder2Repository _orderRepo;

        public AdminController(IProductInterface prodRepo, IOrder2Repository orderRepo)
        {
            _prodRepo = prodRepo;
            _orderRepo = orderRepo;
        }

        public IActionResult AdminIndex()
        {
            return View();
        }

        public IActionResult AdminProducts()
        {
            var products = _prodRepo.Products.ToList();

            return View(products);
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var recordToEdit = _prodRepo.Products.Single(x => x.ProductId == id);

            //ViewBag.Categories = _prodRepo.Categories.ToList();

            return View("addProduct", recordToEdit);
        }

        //[HttpPost]
        //public IActionResult EditProduct(Product updatedMovie)
        //{
        //    _prodRepo.Update(updatedMovie);
        //    _prodRepo.SaveChanges();

        //    return RedirectToAction("AdminProducts");
        //}



        public IActionResult AdminOrdersView(int pageNum)
        {
            int pageSize = 20;

            if (pageNum < 1)
            {
                pageNum = 1;
            }
            var orders = _orderRepo.Orders.Include(o => o.Customer).ToList(); //Where(o => EF.Functions.DateDiffDay(o.Date, DateTime.Today) <= 30).

            var ordersBlah = new OrdersListViewModel
            {
                Orders = _orderRepo.Orders
                    .Where(x => x.Fraud == true) //&& Convert.ToDateTime(x.Date) >= DateTime.Today.AddDays(-30)
                    .OrderByDescending(x => x.Date)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize),
                PaginationInfo = new PaginationInfo
                {
                    currentPage = pageNum,
                    itemsPerPage = pageSize,
                    totalItems = 50
                }
            };

            return View(ordersBlah);
        }
    }
}
