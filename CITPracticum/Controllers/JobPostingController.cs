using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class JobPostingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
