using Microsoft.AspNetCore.Mvc;
using SecurityLab.Models;

namespace SecurityLab.Components
{
    public class ProjectTypesViewComponent : ViewComponent
    {
        private IProductInterface _bookRepo;
        public ProjectTypesViewComponent(IProductInterface temp)
        {
            _bookRepo = temp;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedBookType = RouteData?.Values["productType"];

            var productTypes = _bookRepo.Products
                .Select(x => x.Category1)
                .Distinct()
                .OrderBy(x => x);

            return View(productTypes);
        }
    }
}
