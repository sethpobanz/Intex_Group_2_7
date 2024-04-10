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
