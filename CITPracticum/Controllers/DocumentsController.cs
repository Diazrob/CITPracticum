﻿using CITPracticum.Interfaces;
using CITPracticum.Models;
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
