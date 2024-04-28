using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using CsvHelper;
using System.Globalization;

namespace CITPracticum.Controllers
{
    public class StudentController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<AppUser> _userManager;
        private readonly IStudentRepository _studentRepository;
        private readonly IPlacementRepository _placementRepository;

        public StudentController(UserManager<AppUser> userManager, IStudentRepository studentRepository, IPlacementRepository placementRepository, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _studentRepository = studentRepository;
            _placementRepository = placementRepository;
            _environment = environment;
        }

        // Displays all the students on index page
        public async Task<IActionResult> Index(string sortOrder, string nameFilter, string usernameFilter, string emailFilter, string studIdFilter, int page = 1, int pageSize = 8)
        {
            // Set the page name for breadcrumbs
            ViewData["ActivePage"] = "Student";

            // Filter view variables
            ViewData["CurrentNameFilter"] = nameFilter;
            ViewData["CurrentUsernameFilter"] = usernameFilter;
            ViewData["CurrentEmailFilter"] = emailFilter;
            ViewData["CurrentStudIdFilter"] = studIdFilter;

            // Sort view variables
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["UsernameSortParm"] = sortOrder == "username" ? "username_desc" : "username";
            ViewData["EmailSortParm"] = sortOrder == "email" ? "email_desc" : "email";
            ViewData["StudentIdSortParm"] = sortOrder == "studentid" ? "studentid_desc" : "studentid";

            // Grab users, and students from database
            var users = _userManager.Users.Where(u => u.StudentId.HasValue);
            var students = await _studentRepository.GetAll();

            // Assign those users with the students
            foreach (var user in users)
            {
                foreach (var stud in students)
                {
                    if (user.StudentId == stud.Id)
                    {
                        user.Student = stud;
                    }
                }
            }

            // Apply filters
            if (!string.IsNullOrEmpty(nameFilter))
            {
                users = users.Where(u => u.Student.FirstName.Contains(nameFilter));
            }
            if (!string.IsNullOrEmpty(usernameFilter))
            {
                users = users.Where(u => u.UserName.Contains(usernameFilter));
            }
            if (!string.IsNullOrEmpty(emailFilter))
            {
                users = users.Where(u => u.Email.Contains(emailFilter));
            }
            if (!string.IsNullOrEmpty(studIdFilter))
            {
                users = users.Where(u => u.Student.StuId.Contains(studIdFilter));
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.Student.FirstName);
                    break;
                case "username":
                    users = users.OrderBy(u => u.UserName);
                    break;
                case "username_desc":
                    users = users.OrderByDescending(u => u.UserName);
                    break;
                case "email":
                    users = users.OrderBy(u => u.Email);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(u => u.Email);
                    break;
                case "studentid":
                    users = users.OrderBy(u => u.Student.StuId);
                    break;
                case "studentid_desc":
                    users = users.OrderByDescending(u => u.Student.StuId);
                    break;
                default:
                    users = users.OrderBy(u => u.Student.FirstName);
                    break;
            }

            // Return student list to page
            return View(users.ToList());
        }

        // Upload CSV logic
        [HttpPost]
        public async Task<IActionResult> uploadCSV(IFormFile file)
        {
            // If the file is valid, continue
            if (file != null && file.Length > 0)
            {
                // Get the folder directory
                var filesFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files\\";

                // Create if not found
                if (!Directory.Exists(filesFolder))
                {
                    Directory.CreateDirectory(filesFolder);
                }

                // Combine file name with path
                var filePath = Path.Combine(filesFolder, file.FileName);

                // Create new file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                // Get student list from function return
                var fileStudents = this.GetStudentList(file.FileName);

                // Loop to add each student and user to the database
                foreach (var fileStudent in fileStudents)
                {
                    // Grab user
                    var user = await _userManager.FindByEmailAsync(fileStudent.Email);
                    // Get new student id
                    string updatedStuId = "s0" + fileStudent.StuId;
                    // Get new student name
                    string[] SName = fileStudent.StuName.Split();

                    // Check if user was null
                    if (user == null)
                    {
                        // Create user
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

                        // Create user, return bool
                        var newUserResponse = await _userManager.CreateAsync(newUser, updatedStuId + "College!");

                        // Create new placement and add student id
                        var placement = new Placement()
                        {
                            StudentId = newUser.Student.Id,
                        };

                        // Add to database
                        _placementRepository.Add(placement);

                        // Add role if successful
                        if (newUserResponse.Succeeded)
                            await _userManager.AddToRoleAsync(newUser, UserRoles.Student);
                    }
                }

                // Display a success message
                TempData["Success"] = "File Uploaded. Students Added Successfully.";

                // Return to student index page
                return RedirectToAction("Index", "Student");
            }

            // Display an error message
            TempData["Error"] = "There was an error with the uploaded file. Please ensure the file is correctly formatted.";

            // Return to student index page
            return RedirectToAction("Index", "Student");
        }

        // CSV reader
        private List<studentCSV> GetStudentList(string fileName)
        {
            // Create a list of the students expected from the file
            List<studentCSV> students = new List<studentCSV>();

            // Find the path for the file
            var path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fileName;

            // Read and parse the file for students in the csv file
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

            // Return the list of students back to called area
            return students;
        }

        // Create student page
        public IActionResult Register()
        {
            // Set page name for breadcrumbs
            ViewData["ActivePage"] = "Student";

            // Create view model for form entry
            var response = new RegisterStudentViewModel();

            // Return to the view for user
            return View(response);
        }

        // Register the student to the database
        [HttpPost]
        public async Task<IActionResult> Register(RegisterStudentViewModel registerVM)
        {
            // Check for errors in the form
            if (!ModelState.IsValid) return View(registerVM);

            // Grab the user from the database
            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);

            // Check if user is in system already
            if (user != null)
            {
                // Display an error message
                TempData["Error"] = "This email address is already in use";

                // Return the form, and errors back to user
                return View(registerVM);
            }

            // Create new user and student
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

            // Create in database, return bool
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            // Create a new placement
            var placement = new Placement()
            {
                StudentId = newUser.Student.Id,
            };

            // Add the placement to the database
            _placementRepository.Add(placement);

            // If created, add role to user
            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.Student);

            // Display a success message
            TempData["Success"] = "Student Added Successfully.";

            // Return user back to student page
            return RedirectToAction("Index", "Student");
        }

        // Display student profile/detail page
        public async Task<IActionResult> Detail(int id)
        {
            // Set the page name for breadcrumbs
            ViewData["ActivePage"] = "Student";
            // Grab current user
            var usr = await _userManager.GetUserAsync(User);
            // Grab all student users
            var users = await _userManager.GetUsersInRoleAsync(UserRoles.Student);
            // Find the student with the given id
            Student student = await _studentRepository.GetByIdAsync(id);

            // Assign user to the student id
            foreach (var selStudent in users)
            {
                if (selStudent.StudentId == student.Id)
                {
                    usr = selStudent;
                }
            }

            // Create the profile view model
            var detailStudentVM = new DetailStudentViewModel()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                StuId = student.StuId,
                StuEmail = student.StuEmail,
                User = usr
            };

            // Display the profile view model to user
            return View(detailStudentVM);
        }

        // Reset password
        public async Task<IActionResult> ResetPassword(string email, int id)
        {
            // Grab user using email
            var user = await _userManager.FindByEmailAsync(email);
            // Find the student in the database
            Student student = await _studentRepository.GetByIdAsync(id);
            // Generate password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // Attempt to reset password, return bool
            var result = await _userManager.ResetPasswordAsync(user, token, student.StuId.ToString() + "College!");

            // Display a success message
            TempData["Success"] = "Student password has been reset.";

            // Return to student index page
            return RedirectToAction("Index", "Student");
        }

        // Change password
        [HttpPost]
        public async Task<IActionResult> ChangePassword(DetailStudentViewModel detailStudentVM)
        {
            // Grab user using given email
            var user = await _userManager.FindByEmailAsync(detailStudentVM.StuEmail);
            ModelState.Remove("User");

            // Check if there were any errors on the form
            if (!ModelState.IsValid)
            {
                // Display error message
                TempData["Error"] = "The provided passwords did not match. Please try again.";
                // Redirect to the profile page
                return RedirectToAction("Detail", "Student");
            }

            // 
            TempData["Success"] = "Password was changed successfully.";
            await _userManager.ChangePasswordAsync(user, detailStudentVM.OldPassword, detailStudentVM.Password);

            return RedirectToAction("Detail", "Student");
        }

        // Delete student confirmation page
        public async Task<IActionResult> Delete(string email, int id)
        {
            // Set the page name for breadcrumbs
            ViewData["ActivePage"] = "Student";
            // Grab user with given email
            AppUser user = await _userManager.FindByEmailAsync(email);
            // Grab student with given id
            Student student = await _studentRepository.GetByIdAsync(id);
            // Set the student for the user for model on view
            user.Student = student;
            // Check if user was not in the system
            if (user == null)
            {
                // Display error message
                TempData["Error"] = "The user was not found.";

                // Redirect to student page
                return RedirectToAction("Index", "Student");
            }

            // Display user to view
            return View(user);
        }

        // Delete student logic
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteStudent(string email, int id)
        {
            // Set the page name for bread crumbs
            ViewData["ActivePage"] = "Student";
            // Grab user from the database using the given email
            AppUser user = await _userManager.FindByEmailAsync(email);
            // Grab the student with given id
            Student student = await _studentRepository.GetByIdAsync(id);
            // Display error
            if (user == null)
            {
                // Display error message
                TempData["Error"] = "The user was not found.";

                // Redirect to student page
                return RedirectToAction("Index", "Student");
            }

            // Delete student, then user
            _studentRepository.Delete(student);
            await _userManager.DeleteAsync(user);

            // Display success message
            TempData["Success"] = "Student deleted successfully.";

            // Return back to the student index page
            return RedirectToAction("Index");
        }

        // Edit student view
        public async Task<IActionResult> Edit(int id)
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Student";
            // Grab student with given id
            var student = await _studentRepository.GetByIdAsync(id);


            // Check if student was found
            if (student == null)
            {
                // Display error message
                TempData["Error"] = "The student was not found.";

                // Redirect to student page
                return RedirectToAction("Index", "Student");
            }

            // Create view model for view
            var studentVM = new EditStudentViewModel()
            {
                Id = id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                StuId = student.StuId,
                StuEmail = student.StuEmail
            };

            // Display view model to user
            return View(studentVM);
        }
        
        // Edit student logic
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditStudentViewModel studentVM)
        {
            // Set the page name for breadcrumbs
            ViewData["ActivePage"] = "Student";

            // Check if there are any form errors or invalid entries
            if (!ModelState.IsValid)
            {
                // Display error message
                TempData["Error"] = "Not all entries were valid.";

                ModelState.AddModelError("", "Failed to edit Student");
                return View("Edit", studentVM);
            }

            // Grab student from database with given id
            var curStudent = await _studentRepository.GetIdAsyncNoTracking(id);

            // Check if student was found
            if (curStudent != null)
            {
                // Grab user with email
                var user = await _userManager.FindByEmailAsync(curStudent.StuEmail);
                user.Email = studentVM.StuEmail;
                user.NormalizedEmail = studentVM.StuEmail.ToUpper();

                // Create student with new information
                var student = new Student()
                {
                    Id = id,
                    FirstName = studentVM.FirstName,
                    LastName = studentVM.LastName,
                    StuId = studentVM.StuId,
                    StuEmail = studentVM.StuEmail
                };

                // Update student, and then user
                _studentRepository.Update(student);
                await _userManager.UpdateAsync(user);

                // Display a success message
                TempData["Success"] = "Student edited successfully.";

                // Return to student index page
                return RedirectToAction("Index");
            }
            else
            {
                // Display error message
                TempData["Error"] = "Student was not found.";

                // Return to same page with errors
                return View(studentVM);
            }
        }

        // Upload profile picture logic
        [HttpPost]
        public async Task<IActionResult> UploadPFP(IFormFile profilePicture)
        {
            // Grab current user and student
            var user = await _userManager.GetUserAsync(User);
            var student = await _studentRepository.GetByIdAsync((Int32)user.StudentId);

            // Check for valid photo
            if (profilePicture != null && profilePicture.Length > 0)
            {
                // Generate a unique name for the file
                var fileName = student.StuId + Path.GetExtension(".png");

                // Define the path to save the file to
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads/images");

                // Ensure the directory exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Save the file in this location
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



                // Send back to their profile
                return RedirectToAction("Detail", new { id = user.StudentId });
            }

            // Display error message
            TempData["Error"] = "There was an error setting the profile picture.";

            // Return to the profile page
            return RedirectToAction("Detail", new { id = user.StudentId });
        }
    }
}