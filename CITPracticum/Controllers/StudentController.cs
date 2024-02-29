using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
