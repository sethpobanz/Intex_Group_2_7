using Microsoft.AspNetCore.Mvc;
using SecurityLab.Models;

namespace SecurityLab.Components
{
    public class ColorFilterViewComponent : ViewComponent
    {
        private IProductInterface _repo;
        public ColorFilterViewComponent(IProductInterface temp)
        {
            _repo = temp;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedColor = RouteData?.Values["colors"];


            var colors = _repo.Products
            .Select(x => x.PrimaryColor) // Concatenate the colors
            .Distinct()
            .OrderBy(x => x);
    
            return View(colors);
        }
    }
}
