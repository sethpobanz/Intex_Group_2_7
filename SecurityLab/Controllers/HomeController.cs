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
        private IProductRecInterface _recProdRepo;

        public HomeController(UserManager<IdentityUser> userManager, IProductInterface repo, IUserRecInterface recRepo, ICustomerRepository customerRepo)
        public HomeController(IProductInterface repo, IUserRecInterface recRepo, IProductRecInterface recProdRepo)
        {
            _repo = repo;
            _recRepo = recRepo;
            _customerRepo = customerRepo;
            _userManager = userManager;
            _recProdRepo = recProdRepo;
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


        public IActionResult LegoSingle(int productId)
        {
            // Find the product by productId
            var product = _repo.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                // Product not found, handle error or redirect to another page
                return NotFound(); // Return a 404 Not Found error
            }

            // Find recommendations (Rec1 to Rec5) for the given productId
            var recommendations = _recProdRepo.ProductPipelines.FirstOrDefault(p => p.ProductId == productId);

            if (recommendations != null)
            {
                // Fetch products corresponding to each recommendation
                var recommendedProducts = new List<Product>();

                if (recommendations.Rec1 != null)
                {
                    var rec1Product = _repo.Products.FirstOrDefault(p => p.ProductId == recommendations.Rec1);
                    if (rec1Product != null)
                        recommendedProducts.Add(rec1Product);
                }

                if (recommendations.Rec2 != null)
                {
                    var rec2Product = _repo.Products.FirstOrDefault(p => p.ProductId == recommendations.Rec2);
                    if (rec2Product != null)
                        recommendedProducts.Add(rec2Product);
                }

                if (recommendations.Rec3 != null)
                {
                    var rec3Product = _repo.Products.FirstOrDefault(p => p.ProductId == recommendations.Rec3);
                    if (rec3Product != null)
                        recommendedProducts.Add(rec3Product);
                }

                if (recommendations.Rec4 != null)
                {
                    var rec4Product = _repo.Products.FirstOrDefault(p => p.ProductId == recommendations.Rec4);
                    if (rec4Product != null)
                        recommendedProducts.Add(rec4Product);
                }

                if (recommendations.Rec5 != null)
                {
                    var rec5Product = _repo.Products.FirstOrDefault(p => p.ProductId == recommendations.Rec5);
                    if (rec5Product != null)
                        recommendedProducts.Add(rec5Product);
                }
                if (recommendations.Rec6 != null)
                {
                    var rec6Product = _repo.Products.FirstOrDefault(p => p.ProductId == recommendations.Rec6);
                    if (rec6Product != null)
                        recommendedProducts.Add(rec6Product);
                }

                // Create a view model to pass both product details and recommendations to the view
                var viewModel = new LegoSingleViewModel
                {
                    Product = product,
                    Recommendations = recommendations,
                    RecommendedProducts = recommendedProducts
                };

                return View(viewModel); // Pass the view model to the LegoSingle view
            }

            return View(new LegoSingleViewModel { Product = product, Recommendations = recommendations });
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

    



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
