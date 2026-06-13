using Microsoft.AspNetCore.Mvc;

namespace FrontRailwayCateringERP.Controllers
{
    public class HomeController : Controller
    {
        // GET — Dashboard page
        public IActionResult Get()
        {
            ViewData["Title"] = "Dashboard";
            ViewData["ActivePage"] = "Dashboard";
            return View();
        }

        // Error page — keep this for error handling
        public IActionResult Error()
        {
            return View();
        }
    }
}