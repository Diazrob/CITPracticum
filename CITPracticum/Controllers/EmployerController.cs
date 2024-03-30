using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.Repository;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CITPracticum.Controllers
{
    public class EmployerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmployerRepository _employerRepository;

        // employer constructor
        public EmployerController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context, IEmployerRepository employerRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _employerRepository = employerRepository;
        }
        // displays all the employers
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Employer";
            string roleName = "employer";

            var users = await _userManager.GetUsersInRoleAsync(roleName);
            IEnumerable<Employer> employers = await _employerRepository.GetAll();

            foreach (var employer in employers)
            {
                foreach (var user in users)
                {
                    if (user.EmployerId == employer.Id)
                    {
                        user.Employer.FirstName = employer.FirstName;
                        user.Employer.LastName = employer.LastName;
                        user.Employer.CompanyName = employer.CompanyName;
                    }
                }
            }
            return View(users);
        }
        // function to register a new employer
        public IActionResult Register()
        {
            ViewData["ActivePage"] = "Employer";
            var response = new RegisterEmployerViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterEmployerViewModel registerVM)
        {
            ViewData["ActivePage"] = "Employer";
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if(user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            } 

            var newUser = new AppUser()
            {
                Email = registerVM.EmailAddress,
                UserName = registerVM.FirstName,
                Employer = new Employer()
                {
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName,
                    EmpEmail = registerVM.EmailAddress,
                    CompanyName = registerVM.CompanyName
                }
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.Employer);

            return RedirectToAction("Index", "Employer");
        }
        // shows the page of specific user
        public async Task<IActionResult> Detail(int id)
        {
            ViewData["ActivePage"] = "Employer";
            Employer employer= await _employerRepository.GetByIdAsync(id);
            return View(employer);
        }

        // deletes an employer user
        public async Task<IActionResult> Delete(string email, int id)
        {
            ViewData["ActivePage"] = "Employer";
            AppUser user = await _userManager.FindByEmailAsync(email);
            Employer employer = await _employerRepository.GetByIdAsync(id);
            user.Employer.FirstName = employer.FirstName;
            user.Employer.LastName = employer.LastName;
            user.Employer.Id = employer.Id;
            if (user == null) return View("Error");
            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteEmployer(string email, int id)
        {
            ViewData["ActivePage"] = "Employer";
            AppUser user = await _userManager.FindByEmailAsync(email);
            Employer employer = await _employerRepository.GetByIdAsync(id);
            if (user == null) return View("Error");

            await _userManager.DeleteAsync(user);
            _employerRepository.Delete(employer);

            return RedirectToAction("index");
        }

        // edit an employer
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["ActivePage"] = "Employer";
            var employer = await _employerRepository.GetByIdAsync(id);
            if (employer == null) return View("Error");
            var employerVM = new EditEmployerViewModel()
            {
                Id = id,
                FirstName = employer.FirstName,
                LastName = employer.LastName,
                CompanyName = employer.CompanyName,
                EmpEmail = employer.EmpEmail
            };
            return View(employerVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditEmployerViewModel employerVM)
        {
            ViewData["ActivePage"] = "Employer";
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Employer");
                return View("Edit", employerVM);
            }
            var curEmployer = await _employerRepository.GetIdAsyncNoTracking(id);

            if (curEmployer != null)
            {
                var user = await _userManager.FindByEmailAsync(curEmployer.EmpEmail);
                user.Email = employerVM.EmpEmail;
                user.NormalizedEmail = employerVM.EmpEmail.ToUpper();

                var employer = new Employer()
                {
                    Id = id,
                    FirstName = employerVM.FirstName,
                    LastName = employerVM.LastName,
                    CompanyName = employerVM.CompanyName,
                    EmpEmail = employerVM.EmpEmail
                };
                _employerRepository.Update(employer);
                _userManager.UpdateAsync(user);

                return RedirectToAction("Index");
            }
            else
            {
                return View(employerVM);
            }
        }
    }
}
