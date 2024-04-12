using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SecurityLab.Models;
using SecurityLab.Models.ViewModels;
using System.Diagnostics;

namespace SecurityLab.Controllers
{
    public class HomeController : Controller
    {
        private IProductInterface _repo;
        private IUserRecInterface _recRepo;
        private ICustomerRepository _customerRepo;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(UserManager<IdentityUser> userManager, IProductInterface repo, IUserRecInterface recRepo, ICustomerRepository customerRepo)
        {
            _repo = repo;
            _recRepo = recRepo;
            _customerRepo = customerRepo;
            _userManager = userManager;
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

        public IActionResult LegoList(string? legoType, string? legoColor, int pageNum = 1)
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
                    .Where(x =>
                        (legoType == null || x.Category1 == legoType || x.Category2 == legoType || x.Category3 == legoType) &&
                        (legoColor == null || x.PrimaryColor == legoColor || x.SecondaryColor == legoColor)
                    )
                    .OrderBy(x => x.Name)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize),
                PaginationInfo = new PaginationInfo
                {
                    currentPage = pageNum,
                    itemsPerPage = pageSize,
                    totalItems = _repo.Products.Count()
                },
                CurrentProductType = legoType,
                CurrentColor = legoColor
            };

            return View(blah);
        }

        [HttpGet]
        public IActionResult UserProfile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserProfile(Customer customer)
        {
            if (ModelState.IsValid)
            {
                // Get the current user's ID
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    var existingCustomer = await _customerRepo.Customers.FirstOrDefaultAsync(c => c.UserId == currentUser.Id);
                    if (existingCustomer != null)
                    {
                        // Update existing Customer record
                        existingCustomer.FirstName = customer.FirstName;
                        // ... (update other properties)

                        _customerRepo.Update(existingCustomer);
                    }
                    else { 
                    // Create a new Customer record
                    var maxCustomerId = await _customerRepo.Customers.MaxAsync(c => (int?)c.CustomerId) ?? 0;
                    var newCustomer = new Customer
                    {
                        CustomerId = maxCustomerId + 1, // Set the CustomerId
                        UserId = currentUser.Id,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Gender = customer.Gender,
                        CountryOfResidence = customer.CountryOfResidence
                    };

                    _customerRepo.Add(newCustomer);
                }

                _customerRepo.SaveChanges();
                return RedirectToAction("Success");
                }
            }
            return View(customer);
        }

        public IActionResult Success() 
        { 
            return View(); 
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
