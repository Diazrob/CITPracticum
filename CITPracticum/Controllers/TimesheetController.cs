using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class TimesheetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewTimesheet()
        {
            return View();
        }
        public IActionResult Timestamp()
        {
            return View();
        }
    }
}
