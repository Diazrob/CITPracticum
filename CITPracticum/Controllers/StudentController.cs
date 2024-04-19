using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.Repository;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Hosting;

namespace CITPracticum.Controllers
{
    public class StudentController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IStudentRepository _studentRepository;
        private readonly IPlacementRepository _placementRepository;

        public StudentController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context, IStudentRepository studentRepository, IPlacementRepository placementRepository, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _studentRepository = studentRepository;
            _placementRepository = placementRepository;
            _environment = environment;
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

            var placement = new Placement()
            {
                StudentId = newUser.Student.Id,
            };

            _placementRepository.Add(placement);

            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.Student);

            TempData["Success"] = "Student created successfully.";

            return RedirectToAction("Index", "Student");
        }
        // displays page of a student
        public async Task<IActionResult> Detail(int id)
        {
            ViewData["ActivePage"] = "Student";
            var users = await _userManager.GetUsersInRoleAsync(UserRoles.Student);
            Student student = await _studentRepository.GetByIdAsync(id);
            var user = new AppUser();

            if (student == null)
            {
                TempData["Error"] = "Student profile not found.";
                return RedirectToAction("Index");
            }

            foreach (var selectedUser in users)
            {
                if (student.Id == selectedUser.StudentId)
                {
                    user = await _userManager.FindByEmailAsync(selectedUser.Email);
                    break;
                }
            }

            var studentVM = new ViewStudentViewModel
            {
                Student = student,
                User = user
            };

            if (student != null)
            {
                if (User.IsInRole("student"))
                {
                    if (student.Id != user.StudentId)
                    {
                        return RedirectToAction("Detail", new { id = user.StudentId });
                    }
                }
                return View(studentVM);
            }
            else
            {
                if (User.IsInRole("student"))
                {
                    return RedirectToAction("Detail", new { id = user.StudentId });
                }
                TempData["Error"] = "Student profile not found.";
                return RedirectToAction("Index");
            }
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

            TempData["Success"] = "Student deleted successfully.";

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
                await _userManager.UpdateAsync(user);

                TempData["Success"] = "Student edited successfully.";

                return RedirectToAction("Index");
            }
            else
            {
                return View(studentVM);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadPFP(IFormFile profilePicture)
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _studentRepository.GetByIdAsync((Int32)user.StudentId);

            if (profilePicture != null && profilePicture.Length > 0)
            {
                // Generate a unique name for the file
                var fileName = student.StuId + Path.GetExtension(".png");

                // Define the path to save the file to. For example, "wwwroot/uploads/"
                // Use the web root path to create the save path
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads/images");

                // Ensure the directory exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var savePath = Path.Combine(uploadsFolder, fileName);

                // Save the file
                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(fileStream);
                }

                // Save the relative path (relative to the web root) in the user's record
                user.ProfileImage = Path.Combine("uploads/images", fileName);

                // Update the user
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Detail", new { id = user.StudentId });
            }

            return RedirectToAction("Index");
        }
    }
}