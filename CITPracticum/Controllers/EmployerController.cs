using CITPracticum.Data;
using CITPracticum.Models;
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

        // employer constructor
        public EmployerController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        // displays all the employers
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Employer";
            string roleName = "employer";

            var users = await _userManager.GetUsersInRoleAsync(roleName);
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
                UserName = registerVM.UserName,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                CompanyName = registerVM.CompanyName
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.Employer);

            return RedirectToAction("Index", "Employer");
        }
        // shows the page of specific user
        public async Task<IActionResult> Detail(string email)
        {
            ViewData["ActivePage"] = "Employer";
            AppUser user = await _userManager.FindByEmailAsync(email);
            return View(user);
        }

        // deletes a specific user
        public async Task<IActionResult> Delete(string email)
        {
            ViewData["ActivePage"] = "Employer";
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) return View("Error");
            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteEmployer(string email)
        {
            ViewData["ActivePage"] = "Employer";
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) return View("Error");

            _userManager.DeleteAsync(user);

            return RedirectToAction("index");
        }
    }
}
