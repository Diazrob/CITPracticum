using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
