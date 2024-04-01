using CITPracticum.Interfaces;
using CITPracticum.Models;
using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class TimesheetController : Controller
    {
        private readonly IPlacementRepository _placementRepository;

        public TimesheetController(IPlacementRepository placementRepository)
        {
            _placementRepository = placementRepository;
        }
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Timesheet";
            return View();
        }
        public async Task<IActionResult> ViewTimesheet(int id)
        {
            ViewData["ActivePage"] = "Timesheet";

            Placement placement = await _placementRepository.GetByIdAsync(id);

            if (placement?.Timesheet != null)
            {
                //placement.Timesheet.TotalHours = placement.Timesheet.TimeEntries?.Sum(entry => entry.Hours) ?? 0;
            }

            return View(placement);
        }
        public IActionResult CreateTimeEntry()
        {
            return View();
        }
        public IActionResult Timestamp()
        {
            return View();
        }
    }
}
