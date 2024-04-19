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
        private readonly IEmployerRepository _employerRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IStudentRepository _studentRepository;
        private readonly IAddressRepository _addressRepository;
        public DashboardController(
            ApplicationDbContext context,
            IPracticumFormsRepository practicumFormsRepository,
            IPlacementRepository placementRepository,
            IEmployerRepository employerRepository,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IStudentRepository studentRepository,
            IAddressRepository addressRepository
            ) 
        {
            _context = context;
            _practicumFormsRepository = practicumFormsRepository;
            _placementRepository = placementRepository;
            _employerRepository = employerRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _studentRepository = studentRepository;
            _addressRepository = addressRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Dashboard";

            IEnumerable<Placement> placements = await _placementRepository.GetAll();
            IEnumerable<FormFOIP> formFOIPs = await _practicumFormsRepository.GetAllFormFOIP();
            IEnumerable<FormStuInfo> formIds = await _practicumFormsRepository.GetAllFormStuInfo();
            IEnumerable<FormA> formAs = await _practicumFormsRepository.GetAllFormA();
            IEnumerable<FormB> formBs = await _practicumFormsRepository.GetAllFormB();
            IEnumerable<FormC> formCs = await _practicumFormsRepository.GetAllFormC();
            IEnumerable<FormD> formDs = await _practicumFormsRepository.GetAllFormD();

            

            var usr = await _userManager.GetUserAsync(User);

            if (User.IsInRole("admin"))
            {
                int placementCount = placements.Count();
                int formFOIPCount = formFOIPs.Count();
                int formIdCount = formIds.Count();
                int formACount = formAs.Count();
                int formBCount = formBs.Count();
                int formCCount = formCs.Count();
                int formDCount = formDs.Count();
                int foipPercentage = (int)(((double)formFOIPCount / placementCount) * 100);
                int idPercentage = (int)(((double)formIdCount / placementCount) * 100);
                int aPercentage = (int)(((double)formACount / placementCount) * 100);
                int bPercentage = (int)(((double)formBCount / placementCount) * 100);
                int cPercentage = (int)(((double)formCCount / placementCount) * 100);
                int dPercentage = (int)(((double)formDCount / placementCount) * 100);
                string formFOIPPercentage = Convert.ToString((int)(((double)formFOIPCount / placementCount) * 100)) + "%";
                string formIdPercentage = Convert.ToString((int)(((double)formIdCount / placementCount) * 100)) + "%";
                string formAPercentage = Convert.ToString((int)(((double)formACount / placementCount) * 100)) + "%";
                string formBPercentage = Convert.ToString((int)(((double)formBCount / placementCount) * 100)) + "%";
                string formCPercentage = Convert.ToString((int)(((double)formCCount / placementCount) * 100)) + "%";
                string formDPercentage = Convert.ToString((int)(((double)formDCount / placementCount) * 100)) + "%";

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
