using Microsoft.AspNetCore.Mvc;
using SecurityLab.Models;

namespace SecurityLab.Components
{
    public class ProjectTypesViewComponent : ViewComponent
    {
        private IProductInterface _repo;
        public ProjectTypesViewComponent(IProductInterface temp)
        {
            _repo = temp;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedBookType = RouteData?.Values["productType"];


            var productTypes = _repo.Products
                .Select(x => x.Category1)
                .Distinct()
                .OrderBy(x => x);

            return View(productTypes);
        }
    }
}
