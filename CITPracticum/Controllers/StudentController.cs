using CITPracticum.Data;
using CITPracticum.Models;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Index()
        {
            string roleName = "student";

            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return View(users);
        }
        public IActionResult Register()
        {
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

        public async Task<IActionResult> Detail(string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            return View(user);
        }
    }
}
