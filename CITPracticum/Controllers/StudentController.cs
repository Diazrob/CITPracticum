using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.Repository;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using CsvHelper;
using System.Globalization;
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

        [HttpPost]
        public async Task<IActionResult> uploadCSV(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filesFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files\\";

                if (!Directory.Exists(filesFolder))
                {
                    Directory.CreateDirectory(filesFolder);
                }

                var filePath = Path.Combine(filesFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var fileStudents = this.GetStudentList(file.FileName);

                foreach (var fileStudent in fileStudents)
                {

                    var user = await _userManager.FindByEmailAsync(fileStudent.Email);
                    string updatedStuId = "s0" + fileStudent.StuId;
                    string[] SName = fileStudent.StuName.Split();

                    if (user == null)
                    {
                        var newUser = new AppUser()
                        {
                            Email = fileStudent.Email,
                            UserName = SName.First(),
                            Student = new Student()
                            {
                                FirstName = SName.First(),
                                LastName = SName.Last(),
                                StuEmail = fileStudent.Email,
                                StuId = updatedStuId
                            }
                        };
                        var newUserResponse = await _userManager.CreateAsync(newUser, updatedStuId + "College!");

                        var placement = new Placement()
                        {
                            StudentId = newUser.Student.Id,
                        };

                        _placementRepository.Add(placement);

                        if (newUserResponse.Succeeded)
                            await _userManager.AddToRoleAsync(newUser, UserRoles.Student);
                    }
                }

                TempData["Message"] = "File Uploaded Successfully. Students Added";
                return RedirectToAction("Index", "Student");
            }
            TempData["Message"] = "File upload error. Please check your file.";
            return RedirectToAction("Index", "Student");
        }

        // CSV reader

        private List<studentCSV> GetStudentList(string fileName)
        {
            List<studentCSV> students = new List<studentCSV>();

            var path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fileName;
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var student = csv.GetRecord<studentCSV>();
                    students.Add(student);
                }
            }
            return students;
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

            TempData["Message"] = "Student Added Successfully.";

            return RedirectToAction("Index", "Student");
        }
        // displays page of a student
        public async Task<IActionResult> Detail(int id)
        {
            var usr = await _userManager.GetUserAsync(User);
            if (User.IsInRole("student"))
            {
                id = Convert.ToInt32(usr.StudentId);
            }
            ViewData["ActivePage"] = "Student";
            var users = await _userManager.GetUsersInRoleAsync(UserRoles.Student);
            Student student = await _studentRepository.GetByIdAsync(id);

            foreach (var selStudent in users)
            {
                if (selStudent.StudentId == student.Id)
                {
                    usr = selStudent;
                }
            }

            var detailStudentVM = new DetailStudentViewModel()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                StuId = student.StuId,
                StuEmail = student.StuEmail,
                User = usr
            };

            return View(detailStudentVM);
        }

        // reset password
        public async Task<IActionResult> ResetPassword(string email, int id)
        {
            var user = await _userManager.FindByEmailAsync(email);
            Student student = await _studentRepository.GetByIdAsync(id);
            string StuId = student.StuId;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, StuId + "College!");

            TempData["Message"] = "Student password has been reset";
            return RedirectToAction("Detail", "Student");
        }
        //change password
        [HttpPost]
        public async Task<IActionResult> ChangePassword(DetailStudentViewModel detailStudentVM)
        {
            var user = await _userManager.FindByEmailAsync(detailStudentVM.StuEmail);

            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Password details do not match. Please try again.";
                return RedirectToAction("Detail", "Student");
            }
            TempData["Message"] = "Password successfully changed.";
            await _userManager.ChangePasswordAsync(user, detailStudentVM.OldPassword, detailStudentVM.Password);

            return RedirectToAction("Detail", "Student");
            //// comment this out for now and check if working
            //          var user = new AppUser();

            //          if (student == null)
            //          {
            //              TempData["Error"] = "Student profile not found.";
            //              return RedirectToAction("Index");
            //          }

            //          foreach (var selectedUser in users)
            //          {
            //              if (student.Id == selectedUser.StudentId)
            //              {
            //                  user = await _userManager.FindByEmailAsync(selectedUser.Email);
            //                  break;
            //              }
            //          }

            //          var studentVM = new ViewStudentViewModel
            //          {
            //              Student = student,
            //              User = user
            //          };

            //          if (student != null)
            //          {
            //              if (User.IsInRole("student"))
            //              {
            //                  if (student.Id != user.StudentId)
            //                  {
            //                      return RedirectToAction("Detail", new { id = user.StudentId });
            //                  }
            //              }
            //              return View(studentVM);
            //          }
            //          else
            //          {
            //              if (User.IsInRole("student"))
            //              {
            //                  return RedirectToAction("Detail", new { id = user.StudentId });
            //              }
            //              TempData["Error"] = "Student profile not found.";
            //              return RedirectToAction("Index");
            //          }
            //end here
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
                ModelState.AddModelError("", "Failed to edit Student");
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