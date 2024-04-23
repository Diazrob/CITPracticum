using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly UserManager<AppUser> _userManager;
        public DocumentsController(IStudentRepository studentRepository, UserManager<AppUser> userManager)
        {
            _studentRepository = studentRepository;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Documents";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> uploadCV(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filesFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\resumes\\";

                if (!Directory.Exists(filesFolder))
                {
                    Directory.CreateDirectory(filesFolder);
                }

                var usr = await _userManager.GetUserAsync(User);
                int stuId = Convert.ToInt32(usr.StudentId);
                var student = await _studentRepository.GetByIdAsync(stuId);
                var usrStuId = student.StuId;

                string resumeName = usrStuId + "_" + file.FileName;

                var filePath = Path.Combine(filesFolder, resumeName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }




                TempData["Message"] = "Resume Uploaded Successfully.";
                return RedirectToAction("Index", "Document");
            }
            TempData["Message"] = "File upload error. Please check your file.";
            return RedirectToAction("Index", "Document");
        }



        [HttpPost]
        public async Task<IActionResult> UploadPdf(IFormFile pdfFile)
        {
            if (pdfFile != null && pdfFile.Length > 0)
            {
                // Define a directory to store the files (e.g., "uploads" folder in wwwroot)
                var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                // Generate a unique name for the file
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(pdfFile.FileName);

                var filePath = Path.Combine(uploadsDirectory, fileName);

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(fileStream);
                }

                // At this point, filePath contains the path to the saved file
                // You can now save this path to your Document object

                // Assuming you have a method to get the current student's Document object
                var userId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null && user.Student != null)
                {
                    // Assuming your Document object has a ResumePath property to store the file path
                    user.Student.Document.Resume = filePath;

                    // Update the student record in the database
                    _studentRepository.Update(user.Student);
                    _studentRepository.Save();
                }
            }

            return RedirectToAction("Index");
        }
        public IActionResult CoverLetter()
        {
            ViewData["ActivePage"] = "Documents";
            return View();
        }
    }
}
