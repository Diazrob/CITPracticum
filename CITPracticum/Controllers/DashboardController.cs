using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Dashboard";
            return View();
        }
    }
}
