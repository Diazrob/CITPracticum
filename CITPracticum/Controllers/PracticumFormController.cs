using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class PracticumFormController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
