using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class EmployerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
