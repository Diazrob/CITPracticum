using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.Repository;
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
        private readonly IStudentRepository _studentRepository;

        public StudentController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context, IStudentRepository studentRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _studentRepository = studentRepository;
        }

        // displays all the students on index page
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Student";
            string roleName = "student";

            var users = await _userManager.GetUsersInRoleAsync(roleName);
            IEnumerable<Student> students = await _studentRepository.GetAll();

            foreach (var student in students)
            {
                foreach (var user in users)
                {
                    if (user.StudentId == student.Id)
                    {
                        user.Student.FirstName = student.FirstName;
                        user.Student.LastName = student.LastName;
                        user.Student.StuId = student.StuId;
                    }
                }
            }


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
                UserName = registerVM.FirstName,
                Student = new Student()
                {
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName,
                    StuEmail = registerVM.EmailAddress,
                    StuId = registerVM.StuId
                }
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.Student);

            return RedirectToAction("Index", "Student");
        }
        // displays page of a student
        public async Task<IActionResult> Detail(int id)
        {
            ViewData["ActivePage"] = "Student";
            Student student = await _studentRepository.GetByIdAsync(id);
            return View(student);
        }
        // deletes a student user
        public async Task<IActionResult> Delete(string email, int id)
        {
            ViewData["ActivePage"] = "Student";
            AppUser user = await _userManager.FindByEmailAsync(email);
            Student student = await _studentRepository.GetByIdAsync(id);
            user.Student.FirstName = student.FirstName;
            user.Student.LastName = student.LastName;
            user.Student.Id = student.Id;
            if (user == null) return View("Error");
            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteStudent(string email, int id)
        {
            ViewData["ActivePage"] = "Student";
            AppUser user = await _userManager.FindByEmailAsync(email);
            Student student = await _studentRepository.GetByIdAsync(id);
            if (user == null) return View("Error");

            await _userManager.DeleteAsync(user);
            _studentRepository.Delete(student);

            return RedirectToAction("index");
        }

        // edit a student
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["ActivePage"] = "Student";
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return View("Error");
            var studentVM = new EditStudentViewModel()
            {
                Id = id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                StuId = student.StuId,
                StuEmail = student.StuEmail
            };
            return View(studentVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditStudentViewModel studentVM)
        {
            ViewData["ActivePage"] = "Student";
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Employer");
                return View("Edit", studentVM);
            }
            var curStudent = await _studentRepository.GetIdAsyncNoTracking(id);

            if (curStudent != null)
            {
                var user = await _userManager.FindByEmailAsync(curStudent.StuEmail);
                user.Email = studentVM.StuEmail;
                user.NormalizedEmail = studentVM.StuEmail.ToUpper();

                var student = new Student()
                {
                    Id = id,
                    FirstName = studentVM.FirstName,
                    LastName = studentVM.LastName,
                    StuId = studentVM.StuId,
                    StuEmail = studentVM.StuEmail
                };
                _studentRepository.Update(student);
                _userManager.UpdateAsync(user);

                return RedirectToAction("Index");
            }
            else
            {
                return View(studentVM);
            }
        }
    }
}