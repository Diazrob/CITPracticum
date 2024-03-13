using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class PracticumFormController : Controller
    {
        public IActionResult Index()
        {
            return View();
            // test branch
        }
        public IActionResult EmployerSubmittedForms()
        {
            return View();
        }
        public IActionResult StudentSubmittedForms()
        {
            return View();
        }
    }
}
