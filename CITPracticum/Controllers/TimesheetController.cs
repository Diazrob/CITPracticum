using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class TimesheetController : Controller
    {
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Timesheet";
            return View();
        }
        public IActionResult ViewTimesheet()
        {
            ViewData["ActivePage"] = "Timesheet";
            return View();
        }
        public IActionResult Timestamp()
        {
            return View();
        }
    }
}
