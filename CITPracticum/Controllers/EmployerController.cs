using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.Repository;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CITPracticum.Controllers
{
    public class EmployerController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IAddressRepository _addressRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmployerRepository _employerRepository;

        // employer constructor
        public EmployerController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context, IEmployerRepository employerRepository, IWebHostEnvironment environment,
            IAddressRepository addressRepository
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _employerRepository = employerRepository;
            _environment = environment;
            _addressRepository = addressRepository;
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
        public async Task<IActionResult> Register(RegisterEmployerViewModel registerVM, List<string> credentialsList)
        {
            ViewData["ActivePage"] = "Employer";

            string credentials = string.Join(", ", credentialsList);

            if (credentials != "")
            {
                ModelState.SetModelValue("Credentials", new ValueProviderResult(credentials, CultureInfo.InvariantCulture));
                registerVM.Credentials = credentials;
                ModelState.Remove("Credentials");
            }

            if (User.IsInRole("student"))
            {
                registerVM.Password = registerVM.LastName + "Employer1!";
                registerVM.ConfirmPassword = registerVM.LastName + "Employer1!";
            }

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
                    CompanyName = registerVM.CompanyName,
                    SVPosition = registerVM.SVPosition,
                    OrgType = registerVM.OrgType,
                    PhoneNumber = registerVM.PhoneNumber,
                    Credentials = registerVM.Credentials,
                    CredOther = registerVM.CredOther,
                    Affiliation = registerVM.Affiliation,
                    Address = new Address()
                    {
                        Street = registerVM.CreateAddressViewModel.Street,
                        City = registerVM.CreateAddressViewModel.City,
                        Prov = registerVM.CreateAddressViewModel.Prov,
                        Country = registerVM.CreateAddressViewModel.Country,
                        PostalCode = registerVM.CreateAddressViewModel.PostalCode,
                    },
                    EmployerComments = registerVM.EmployerComments,
                }
            };
          
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            await _userManager.AddToRoleAsync(newUser, UserRoles.Employer);

            TempData["Success"] = "Employer created successfully.";

            if (User.IsInRole("student"))
            {
                return RedirectToAction("SearchEmployer", "PracticumForm");
            }
            else
            {
                return RedirectToAction("Index", "Employer");
            }

        }

        // shows the page of specific user
        public async Task<IActionResult> Detail(int id)
        {
            var usr = await _userManager.GetUserAsync(User);
            if (User.IsInRole("employer"))
            {
                id = Convert.ToInt32(usr.EmployerId);
            }
            ViewData["ActivePage"] = "Employer";
            var users = await _userManager.GetUsersInRoleAsync(UserRoles.Employer);
            var addresses = await _addressRepository.GetAll();
            Employer employer = await _employerRepository.GetByIdAsync(id);
            int addId = Convert.ToInt32(employer.AddressId);
            Address address = await _addressRepository.GetByIdAsync(addId);

            foreach (var selEmployer in users)
            {
                if (selEmployer.EmployerId == employer.Id)
                {
                    usr = selEmployer;
                }
            }
            foreach (var empAddress in addresses)
            {
                if (empAddress.Id == employer.AddressId)
                {
                    employer.Address.Street = empAddress.Street;
                    employer.Address.City = empAddress.City;
                    employer.Address.Prov = empAddress.Prov;
                    employer.Address.Country = empAddress.Country;
                    employer.Address.PostalCode = empAddress.PostalCode;
                }
            }

            var detailEmployerVM = new DetailEmployerViewModel()
            {
                Id = employer.Id,
                FirstName = employer.FirstName,
                LastName = employer.LastName,
                CompanyName = employer.CompanyName,
                SVPosition = employer.SVPosition,
                OrgType = employer.OrgType,
                EmpEmail = employer.EmpEmail,
                PhoneNumber = employer.PhoneNumber,
                Credentials = employer.Credentials,
                CredOther = employer.CredOther,
                Address = new Address()
                {
                    Street = employer.Address.Street,
                    City = employer.Address.City,
                    Prov = employer.Address.Prov,
                    Country = employer.Address.Country,
                    PostalCode = employer.Address.PostalCode
                },
                User = usr
            };

            return View(detailEmployerVM);
        }

        // reset password
        public async Task<IActionResult> ResetPassword(string email, int id)
        {
            var user = await _userManager.FindByEmailAsync(email);
            Employer employer = await _employerRepository.GetByIdAsync(id);
            

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, employer.LastName + "Employer1!");

            TempData["Message"] = "Employer password has been reset";
            return RedirectToAction("Index", "Employer");
        }
        //change password
        [HttpPost]
        public async Task<IActionResult> ChangePassword(DetailEmployerViewModel detailEmployerVM)
        {
            var user = await _userManager.FindByEmailAsync(detailEmployerVM.EmpEmail);
            ModelState.Remove("User");
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Password details do not match. Please try again.";
                return RedirectToAction("Detail", "Employer");
            }
            TempData["Message"] = "Password successfully changed.";
            await _userManager.ChangePasswordAsync(user, detailEmployerVM.OldPassword, detailEmployerVM.Password);

            return RedirectToAction("Detail", "Employer");
        }

        //var users = await _userManager.GetUsersInRoleAsync(UserRoles.Employer);
        //Employer emp = await _employerRepository.GetByIdAsync(id);
        //var user = new AppUser();

        //if (emp == null)
        //{
        //    TempData["Error"] = "Employer profile not found.";
        //    return RedirectToAction("Index");
        //}

        //foreach (var selectedUser in users)
        //{
        //    if (emp.Id == selectedUser.EmployerId)
        //    {
        //        user = await _userManager.FindByEmailAsync(selectedUser.Email);
        //        break;
        //    }
        //}

        //var empVM = new ViewEmployerViewModel
        //{
        //    Employer = emp,
        //    User = user
        //};

        //if (emp != null)
        //{
        //    if (User.IsInRole("employer"))
        //    {
        //        if (emp.Id != user.EmployerId)
        //        {
        //            return RedirectToAction("Detail", new { id = user.EmployerId });
        //        }
        //    }
        //    return View(empVM);
        //}
        //else
        //{
        //    if (User.IsInRole("employer"))
        //    {
        //        return RedirectToAction("Detail", new { id = user.EmployerId });
        //    }
        //    TempData["Error"] = "Employer profile not found.";
        //    return RedirectToAction("Index");
        //}

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

            TempData["Success"] = "Employer deleted successfully.";

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
                EmpEmail = employer.EmpEmail,
                EmployerComments = employer.EmployerComments,
                Affiliation = employer.Affiliation,
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
                    EmpEmail = employerVM.EmpEmail,
                    EmployerComments = employerVM.EmployerComments,
                    Affiliation = employerVM.Affiliation,
                };
                _employerRepository.Update(employer);
                await _userManager.UpdateAsync(user);

                TempData["Success"] = "Employer edited successfully.";

                return RedirectToAction("Index");
            }
            else
            {
                return View(employerVM);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadPFP(IFormFile profilePicture)
        {
            var user = await _userManager.GetUserAsync(User);
            var employer = await _employerRepository.GetByIdAsync((Int32)user.EmployerId);

            if (profilePicture != null && profilePicture.Length > 0)
            {
                // Generate a unique name for the file
                var fileName = employer.CompanyName + Path.GetExtension(".png");

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
                return RedirectToAction("Detail", new { id = user.EmployerId });
            }

            return RedirectToAction("Index");
        }
    }
}
