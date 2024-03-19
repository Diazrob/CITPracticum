using CITPracticum.Data;
using CITPracticum.Models;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace CITPracticum.Controllers
{
    public class StudentController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public StudentController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // displays all the students on index page
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Student";
            string roleName = "student";

            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return View(users);
        }
        // add a new student user
        public IActionResult Register()
        {
            ViewData["ActivePage"] = "Student";
            var response = new RegisterStudentViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterStudentViewModel registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
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
                StudentId = registerVM.StudentId
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.Student);

            return RedirectToAction("Index", "Student");
        }
        // displays page of a student
        public async Task<IActionResult> Detail(string email)
        {
            ViewData["ActivePage"] = "Student";
            AppUser user = await _userManager.FindByEmailAsync(email);
            return View(user);
        }
        // deletes a student user
        public async Task<IActionResult> Delete(string email)
        {
            ViewData["ActivePage"] = "Student";
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) return View("Error");
            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteStudent(string email)
        {
            ViewData["ActivePage"] = "Student";
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) return View("Error");

            _userManager.DeleteAsync(user);

            return RedirectToAction("index");
        }
    }
}
