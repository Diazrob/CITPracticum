using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class DocumentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
