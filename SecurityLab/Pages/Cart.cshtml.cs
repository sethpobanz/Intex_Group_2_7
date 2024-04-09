using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecurityLab.Infrastructure;
using SecurityLab.Models;

namespace SecurityLab.Pages
{
    public class BuyModel : PageModel
    {
        private IProductInterface _repo;
        public Cart Cart { get; set; }
        public BuyModel(IProductInterface temp, Cart cartService)
        {
            _repo = temp;
            Cart = cartService;
        }

        public string ReturnUrl { get; set; } = "/";

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";

        }

        public IActionResult OnPost(int productId, string returnUrl)
        {
            Product product = _repo.Products
                .FirstOrDefault(x => x.ProductId == productId);

            if (product != null)
            {

                Cart.AddItem(product, 1);

            }

            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostRemove(int productId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(x => x.Product.ProductId == productId).Product);
            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}

