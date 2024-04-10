using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecurityLab.Models;
using SecurityLab.Models.ViewModels;

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
        public IActionResult addProduct()
        {
            return View(new Product());
        }

        [HttpPost]
        public IActionResult addProduct(Product response)
        {
            _prodRepo.Add(response);
            _prodRepo.SaveChanges();
            
            return RedirectToAction("AdminProducts");
        }



        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var recordToEdit = _prodRepo.Products.Single(x => x.ProductId == id);

            //ViewBag.Categories = _prodRepo.Categories.ToList();

            return View("addProduct", recordToEdit);
        }

        [HttpPost]
        public IActionResult EditProduct(Product updatedProduct)
        {
            _prodRepo.Update(updatedProduct);
            _prodRepo.SaveChanges();

            return RedirectToAction("AdminProducts");
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            var productToDelete = _prodRepo.Products.FirstOrDefault(p => p.ProductId == id);

            if (productToDelete == null)
            {
                return NotFound(); // Product not found, return 404 Not Found page
            }

            return View(productToDelete);
        }


        [HttpPost]
        public IActionResult DeleteProduct(Product products)
        {
            _prodRepo.Remove(products);
            _prodRepo.SaveChanges();
            return RedirectToAction("AdminProducts");
        }



        public IActionResult AdminOrdersView(int pageNum)
        {
            int pageSize = 20;

            if (pageNum < 1)
            {
                pageNum = 1;
            }
            var orders = _orderRepo.Orders.Include(o => o.Customer).ToList();

            var ordersBlah = new OrdersListViewModel
            {
                Orders = _orderRepo.Orders
                    .Where(x => x.Fraud == true)     // Math.Abs((x.Date.ToDateTime(new Time(0, 0)) - DateTime.Today).Days) <= 30 ORRRR && x.Date >= DateTime.Now.AddDays(-30)                 
                    .OrderByDescending(x => x.Date)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize),
                PaginationInfo = new PaginationInfo
                {
                    currentPage = pageNum,
                    itemsPerPage = pageSize,
                    totalItems = _orderRepo.Orders.Count()
                }
            };

            return View(ordersBlah);
        }
    }
}
