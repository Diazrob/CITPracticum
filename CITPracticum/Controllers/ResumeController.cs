using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class ResumeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
