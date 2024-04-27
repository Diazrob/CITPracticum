using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPracticumFormsRepository _practicumFormsRepository;
        private readonly IPlacementRepository _placementRepository;
        private readonly UserManager<AppUser> _userManager;
        public DashboardController(
            ApplicationDbContext context,
            IPracticumFormsRepository practicumFormsRepository,
            IPlacementRepository placementRepository,
            UserManager<AppUser> userManager
            )
        {
            _context = context;
            _practicumFormsRepository = practicumFormsRepository;
            _placementRepository = placementRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Set page name for breadcrumbs
            ViewData["ActivePage"] = "Dashboard";

            // Lists of forms and placements
            IEnumerable<Placement> placements = await _placementRepository.GetAll();
            IEnumerable<FormFOIP> formFOIPs = await _practicumFormsRepository.GetAllFormFOIP();
            IEnumerable<FormStuInfo> formIds = await _practicumFormsRepository.GetAllFormStuInfo();
            IEnumerable<FormA> formAs = await _practicumFormsRepository.GetAllFormA();
            IEnumerable<FormB> formBs = await _practicumFormsRepository.GetAllFormB();
            IEnumerable<FormC> formCs = await _practicumFormsRepository.GetAllFormC();
            IEnumerable<FormD> formDs = await _practicumFormsRepository.GetAllFormD();

            // Admin only logic
            if (User.IsInRole("admin"))
            {
                // Grabs the counts of the forms
                int placementCount = placements.Count();
                int formFOIPCount = formFOIPs.Count();
                int formIdCount = formIds.Count();
                int formACount = formAs.Count();
                int formBCount = formBs.Count();
                int formCCount = formCs.Count();
                int formDCount = formDs.Count();

                // Create a percentage for each set of completed forms
                int foipPercentage = (int)(((double)formFOIPCount / placementCount) * 100);
                int idPercentage = (int)(((double)formIdCount / placementCount) * 100);
                int aPercentage = (int)(((double)formACount / placementCount) * 100);
                int bPercentage = (int)(((double)formBCount / placementCount) * 100);
                int cPercentage = (int)(((double)formCCount / placementCount) * 100);
                int dPercentage = (int)(((double)formDCount / placementCount) * 100);
                
                // Check if there are any placements, if there aren't any, set the percentages to zero
                if (placementCount == 0)
                {
                    foipPercentage = 0;
                    idPercentage = 0;
                    aPercentage = 0;
                    bPercentage = 0;
                    cPercentage = 0;
                    dPercentage = 0;
                }

                // Set percentage values to strings for displaying
                string formFOIPPercentage = Convert.ToString(foipPercentage) + "%";
                string formIdPercentage = Convert.ToString(idPercentage) + "%";
                string formAPercentage = Convert.ToString(aPercentage) + "%";
                string formBPercentage = Convert.ToString(bPercentage) + "%";
                string formCPercentage = Convert.ToString(cPercentage) + "%";
                string formDPercentage = Convert.ToString(dPercentage) + "%";
                
                // Create dashboard object, and provide all of the calculated values.
                var dashboardVM = new DashboardViewModel()
                {
                    PlacementCount = placementCount,
                    FormFOIPCount = formFOIPCount,
                    FormIdCount = formIdCount,
                    FormACount = formACount,
                    FormBCount = formBCount,
                    FormCCount = formCCount,
                    FormDCount = formDCount,
                    FOIPPercentage = formFOIPPercentage,
                    IdPercentage = formIdPercentage,
                    APercentage = formAPercentage,
                    BPercentage = formBPercentage,
                    CPercentage = formCPercentage,
                    DPercentage = formDPercentage,
                    FOIPPercent = foipPercentage,
                    IdPercent = idPercentage,
                    APercent = aPercentage,
                    BPercent = bPercentage,
                    CPercent = cPercentage,
                    DPercent = dPercentage
                };

                return View(dashboardVM);
            }
            return View();
        }
    }
}
