using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecurityLab.Models;

namespace SecurityLab.Controllers
{

    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly PobanzTestDbContext _context;
        private Cart cart;

        public OrderController(IOrderRepository repoService,
                Cart cartService, UserManager<IdentityUser> userManager,
                PobanzTestDbContext context)
        {
            repository = repoService;
            cart = cartService;
            _userManager = userManager;
            _context = context;
        }
        [Authorize]
        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("",
                    "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                var maxOrderId = repository.Orders.Max(p => (int?)p.TransactionId) ?? 0;
                order.TransactionId = maxOrderId + 1;

                // Get the current user
                var user = await _userManager.GetUserAsync(User);

                // Check if the customer exists for the current user
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);

                if (customer != null)
                {
                    // Set the existing Customer on the order
                    order.Customer = customer;
                    order.CustomerId = customer.CustomerId;
                    order.Customer.CountryOfResidence = customer.CountryOfResidence;
                }
                else
                {
                    // Create a new Customer record
                    var maxCustomerId = await _context.Customers.MaxAsync(c => (int?)c.CustomerId) ?? 0;
                    var newCustomer = new Customer
                    {
                        CustomerId = maxCustomerId + 1,
                        UserId = user.Id,
                        FirstName = order.Customer.FirstName,
                        LastName = order.Customer.LastName,
                        CountryOfResidence = order.Customer.CountryOfResidence
                        // Set other customer properties as needed
                    };

                    _context.Customers.Add(newCustomer);
                    await _context.SaveChangesAsync();

                    // Set the new Customer on the order
                    order.Customer = newCustomer;
                    order.CustomerId = newCustomer.CustomerId;
                }

                repository.SaveOrder(order);
                cart.Clear();
                return RedirectToPage("/Completed",
                    new { orderId = order.TransactionId });
            }
            else
            {
                return View();
            }
        }
    }
}