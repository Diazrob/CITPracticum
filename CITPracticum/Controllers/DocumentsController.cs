using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class DocumentsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Documents";
            return View();
        }

       
        
    }
}
