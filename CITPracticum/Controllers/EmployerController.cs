using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace CITPracticum.Controllers
{
    public class EmployerController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IAddressRepository _addressRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmployerRepository _employerRepository;

        // Employer constructor
        public EmployerController(UserManager<AppUser> userManager, IEmployerRepository employerRepository, IWebHostEnvironment environment, IAddressRepository addressRepository)
        {
            _userManager = userManager;
            _employerRepository = employerRepository;
            _environment = environment;
            _addressRepository = addressRepository;
        }

        // Displays the employers to the admin side, provides all employers in a table
        public async Task<IActionResult> Index(string sortOrder, string nameFilter, string usernameFilter, string emailFilter, string companyFilter, int page = 1, int pageSize = 8)
        {
            // Set the page name for breadcrumbs
            ViewData["ActivePage"] = "Employer";

            // Set the view data variables to the provided form data
            // This is all used for sorting and filtering
            // Filter values
            ViewData["CurrentNameFilter"] = nameFilter;
            ViewData["CurrentUsernameFilter"] = usernameFilter;
            ViewData["CurrentEmailFilter"] = emailFilter;
            ViewData["CurrentCompanyFilter"] = companyFilter;

            // Sort Values
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["UsernameSortParm"] = sortOrder == "username" ? "username_desc" : "username";
            ViewData["EmailSortParm"] = sortOrder == "email" ? "email_desc" : "email";
            ViewData["CompanySortParm"] = sortOrder == "company" ? "company_desc" : "company";

            // Grab all users that are employers
            var users = _userManager.Users.Where(u => u.EmployerId.HasValue);
            IEnumerable<Employer> employers = await _employerRepository.GetAll();

            // Fill in the employer data for each user in users
            foreach (var user in users)
            {
                foreach (var employer in employers)
                {
                    if (user.EmployerId == employer.Id)
                    {
                        user.Employer = employer;
                        continue;
                    }
                }
            }

            // Check if the first name is associated with any users
            if (!string.IsNullOrEmpty(nameFilter))
            {
                users = users.Where(u => u.Employer.FirstName.Contains(nameFilter));
            }

            // Check if the username is associated with any users
            if (!string.IsNullOrEmpty(usernameFilter))
            {
                users = users.Where(u => u.UserName.Contains(usernameFilter));
            }

            // Check if the email is associated with any users
            if (!string.IsNullOrEmpty(emailFilter))
            {
                users = users.Where(u => u.Email.Contains(emailFilter));
            }

            // Check if the company is associated with any users
            if (!string.IsNullOrEmpty(companyFilter))
            {
                users = users.Where(u => u.Employer.CompanyName.Contains(companyFilter));
            }

            // Apply sorting, (Ex: username = ascending, username_desc = descending)
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.Employer.FirstName);
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
                case "company":
                    users = users.OrderBy(u => u.Employer.CompanyName);
                    break;
                case "company_desc":
                    users = users.OrderByDescending(u => u.Employer.CompanyName);
                    break;
                default:
                    users = users.OrderBy(u => u.Employer.FirstName);
                    break;
            }

            return View(users);
        }

        // Displays the employer registration page
        public IActionResult Register()
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Employer";

            // Create view model for employer registration
            var response = new RegisterEmployerViewModel();
            return View(response);
        }

        // Employer registration, this is after the registration form is submitted
        [HttpPost]
        public async Task<IActionResult> Register(RegisterEmployerViewModel registerVM, List<string> credentialsList)
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Employer";

            string credentials = string.Join(", ", credentialsList);

            if (credentials != "")
            {
                ModelState.SetModelValue("Credentials", new ValueProviderResult(credentials, CultureInfo.InvariantCulture));
                registerVM.Credentials = credentials;
                ModelState.Remove("Credentials");
            }

            // If a student is creating the employer, set a default password.
            // The student should not be allowed to set the employer password.
            if (User.IsInRole("student"))
            {
                registerVM.Password = registerVM.LastName + "Employer1!";
                registerVM.ConfirmPassword = registerVM.LastName + "Employer1!";
            }

            // If there are fields that were invalid or not filled in, return back to the page.
            if (!ModelState.IsValid) return View(registerVM);

            // Attempt to see if there is a user with the email entered into the system.
            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);

            // Display error if the email is already in the system.
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }

            // Create the new user, and assign them to being an employer.
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
            
            // Check if the user was properly created.
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);
            
            // If user was created successfully, then add the role to the user.
            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.Employer);

            // Display message for creating the employer successfully/
            TempData["Success"] = "Employer created successfully.";

            // Return students back to the search employer area
            if (User.IsInRole("student"))
            {
                return RedirectToAction("SearchEmployer", "PracticumForm");
            }
            // Return other users back to the employer list page.
            else
            {
                return RedirectToAction("Index", "Employer");
            }

        }

        // Shows the profile page of a specific employer. 
        // Accessible by admins, and the employers themselves.
        public async Task<IActionResult> Detail(int id)
        {
            // Grab the current logged in user
            var usr = await _userManager.GetUserAsync(User);

            // Set the id to the employer id if they are the logged in user
            if (User.IsInRole("employer"))
            {
                id = Convert.ToInt32(usr.EmployerId);
            }

            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Employer";

            // Grab all employers from the database
            var users = await _userManager.GetUsersInRoleAsync(UserRoles.Employer);
            // Grab all addresses from the database
            var addresses = await _addressRepository.GetAll();
            // Set the employer to the selected employer
            Employer employer = await _employerRepository.GetByIdAsync(id);
            // Set the addressId to the selected employer addressId
            int addId = Convert.ToInt32(employer.AddressId);
            // Set the address to the employer address from the database
            Address address = await _addressRepository.GetByIdAsync(addId);

            // Set the user to the selected employer
            foreach (var selEmployer in users)
            {
                if (selEmployer.EmployerId == employer.Id)
                {
                    usr = selEmployer;
                }
            }

            // Set the employers address information with the data
            // pulled from the database.
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

            // Profile page view model, displays a lot of employer information
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
                EmployerComments = employer.EmployerComments,
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

        // Reset password
        public async Task<IActionResult> ResetPassword(string email, int id)
        {
            // Grab the user with the provided email address
            var user = await _userManager.FindByEmailAsync(email);
            // Find that specific employer
            Employer employer = await _employerRepository.GetByIdAsync(id);

            // Create a password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Store result in boolean, 
            var result = await _userManager.ResetPasswordAsync(user, token, employer.LastName + "Employer1!");

            // Send success or error message if the result was true or false.
            if (result.Succeeded)
            {
                TempData["Message"] = "Employer password has been reset.";
            } else
            {
                TempData["Error"] = "There was an error resetting the employers password.";
            }

            // Display employer page, and possibly send a success or error message along.
            return RedirectToAction("Index", "Employer");
        }

        // Change password
        [HttpPost]
        public async Task<IActionResult> ChangePassword(DetailEmployerViewModel detailEmployerVM)
        {
            // Grab the employer from the database with the given email
            var user = await _userManager.FindByEmailAsync(detailEmployerVM.EmpEmail);
            ModelState.Remove("User");

            // Display error message and return back to the page if there are errors
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "The given passwords did not match. Please try again.";
                return RedirectToAction("Detail", "Employer");
            }

            // Success message
            TempData["Success"] = "Password successfully changed.";

            // Change password in the actual system
            await _userManager.ChangePasswordAsync(user, detailEmployerVM.OldPassword, detailEmployerVM.Password);

            // Return back to the page with the success message.
            return RedirectToAction("Detail", "Employer");
        }

        // Delete confirmation
        public async Task<IActionResult> Delete(string email, int id)
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Employer";

            // Grab the user with the provided email
            AppUser user = await _userManager.FindByEmailAsync(email);

            // Grab the employer with the id from the user
            Employer employer = await _employerRepository.GetByIdAsync(id);

            // Set the employer data to the user.
            user.Employer = employer;

            // If the user was not found, return an error
            if (user == null) return View("Error");

            // Displays the information to the users page.
            return View(user);
        }

        // Delete function after delete button was pressed.
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteEmployer(string email, int id)
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Employer";

            // Grab the user with the given email address.
            AppUser user = await _userManager.FindByEmailAsync(email);

            // Grab the employer with the given id
            Employer employer = await _employerRepository.GetByIdAsync(id);

            // If the user was not found, display an error.
            if (user == null) return View("Error");

            // Attempt to delete the employer from the system
            _employerRepository.Delete(employer);
            // Attempt to delete the user from the system
            await _userManager.DeleteAsync(user);

            // Display a success message
            TempData["Success"] = "Employer deleted successfully.";

            // Redirect user back to index page after deletion
            return RedirectToAction("Index");
        }

        // Edit form information
        public async Task<IActionResult> Edit(int id)
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Employer";

            // Find the employer with the given id
            var employer = await _employerRepository.GetByIdAsync(id);

            // If the employer wasnt found, display an error
            if (employer == null) return View("Error");

            // Create the edit view model for the users page
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

            // Display the view model form to the users page
            return View(employerVM);
        }

        // Edit logic after form was submitted
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditEmployerViewModel employerVM)
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Employer";

            // If there are errors, go back to the page
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Employer");
                return View("Edit", employerVM);
            }
            
            // Grab the employer with the given id from the form submit
            var curEmployer = await _employerRepository.GetIdAsyncNoTracking(id);

            // If the employer was found logic
            if (curEmployer != null)
            {
                // Find the user in the system associated with the employer
                var user = await _userManager.FindByEmailAsync(curEmployer.EmpEmail);

                // Set the email to the new email 
                user.Email = employerVM.EmpEmail;
                user.NormalizedEmail = employerVM.EmpEmail.ToUpper();

                // Set the information of the employer to the new information
                curEmployer.FirstName = employerVM.FirstName;
                curEmployer.LastName = employerVM.LastName;
                curEmployer.CompanyName = employerVM.CompanyName;
                curEmployer.EmpEmail = employerVM.EmpEmail;
                curEmployer.EmployerComments = employerVM.EmployerComments;
                curEmployer.Affiliation = employerVM.Affiliation;

                // Update the employer and user in the database
                _employerRepository.Update(curEmployer);
                await _userManager.UpdateAsync(user);

                // Display a success message
                TempData["Success"] = "Employer edited successfully.";

                // Display the employer page
                return RedirectToAction("Index");
            }
            else
            {
                // Display an error message
                TempData["Error"] = "There was an error finding the employer in the system with the given id.";

                // Return back to the edit form with the given errors
                return View(employerVM);
            }
        }
        
        // Upload profile picture function
        [HttpPost]
        public async Task<IActionResult> UploadPFP(IFormFile profilePicture)
        {
            // Grab the user and employer from the database
            var user = await _userManager.GetUserAsync(User);
            var employer = await _employerRepository.GetByIdAsync((Int32)user.EmployerId);

            // If the given picture has information
            if (profilePicture != null && profilePicture.Length > 0)
            {
                // Generate a unique name for the file
                var fileName = employer.CompanyName + Path.GetExtension(".png");

                // Define the path to save the file to
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads/images");

                // Check if the directory exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Create the save path
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

                // Display a success message
                TempData["Success"] = "Profile picture uploaded successfully.";

                // Send back to the employer page hopefully with their new profile picture
                return RedirectToAction("Detail", new { id = user.EmployerId });
            }

            // Display an error message
            TempData["Error"] = "There was an error uploading the selected image to the system.";

            // Redirect back to the employer list
            return RedirectToAction("Detail", new { id = user.EmployerId });
        }
    }
}
