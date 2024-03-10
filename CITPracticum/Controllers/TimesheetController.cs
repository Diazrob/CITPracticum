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
    }
}
