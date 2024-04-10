using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace SecurityLab.Pages.Roles
{
    [Authorize]
    public class ListModel : PageModel
    {
        
        public void OnGet()
        {
        }
    }
}
