
using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;


namespace CITPracticum.Controllers
{
    public class PracticumFormController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPracticumFormsRepository _practicumFormsRepository;
        private readonly IPlacementRepository _placementRepository;
        private readonly IEmployerRepository _employerRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IStudentRepository _studentRepository;
        private readonly IAddressRepository _addressRepository;

        public PracticumFormController(
            ApplicationDbContext context, 
            IPracticumFormsRepository practicumFormsRepository, 
            IPlacementRepository placementRepository, 
            IEmployerRepository employerRepository, 
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            IStudentRepository studentRepository,
            IAddressRepository addressRepository
            )     
        {
            _context = context;
            _practicumFormsRepository = practicumFormsRepository;
            _placementRepository = placementRepository;
            _employerRepository = employerRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _studentRepository = studentRepository;
            _addressRepository = addressRepository;
        }
        public async Task<IActionResult> Index()
        {
            //if (User.IsInRole("student"))
            //{
            //    var usr = await _userManager.GetUserAsync(User);
            //    int stuId = Convert.ToInt32(usr.StudentId);
            //    var student = await _studentRepository.GetByIdAsync(stuId);
            //    var usrLastName = student.LastName;
            //    var usrFirstName = student.FirstName;
            //    var usrStuId = student.StuId;

            //    var submitFormAVM = new Placement()
            //    {
            //        Student = new Student()
            //        {
            //            LastName = usrLastName,
            //            FirstName = usrFirstName,
            //            StuId = usrStuId
            //        }
            //    };
            //    return View(submitFormAVM);
            //}
            //else
            //{
                var submitFormAVM = new Placement();
                return View(submitFormAVM);
            //}

            //return View();
        }

        public IActionResult EmployerSubmittedForms()
        {
            ViewData["ActivePage"] = "PracForms";
            return View();
        }
        public IActionResult StudentSubmittedForms()
        {
            ViewData["ActivePage"] = "PracForms";
            return View();
        }

        // Form FOIP submission handler
        public async Task<IActionResult> CreateFormFOIP()
        {
            if (User.IsInRole("student"))
            {
                var usr = await _userManager.GetUserAsync(User);
                int stuId = Convert.ToInt32(usr.StudentId);
                var student = await _studentRepository.GetByIdAsync(stuId);
                var usrLastName = student.LastName;
                var usrFirstName = student.FirstName;
                var usrStuId = student.StuId;

                var createFormFOIPViewModel = new CreateFormFOIPViewModel()
                {
                    StuLastName = usrLastName,
                    StuFirstName = usrFirstName,
                    StuId = usrStuId,
                    StuSignDate = DateTime.Now
                };
                return View(createFormFOIPViewModel);
            }
            else
            {
                var createFormFOIPViewModel = new CreateFormFOIPViewModel();
                return View(createFormFOIPViewModel);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateFormFOIP(CreateFormFOIPViewModel formFOIPViewModel)
        {

            var usr = await _userManager.GetUserAsync(User);
            int stuId = Convert.ToInt32(usr.StudentId);

            if (ModelState.IsValid)
            {

                var placement = new Placement()
                {
                    PracticumForms = new PracticumForms()
                    {
                        FormFOIP = new FormFOIP()
                        {
                            StuFirstName = formFOIPViewModel.StuFirstName,
                            StuLastName = formFOIPViewModel.StuLastName,
                            StuId = formFOIPViewModel.StuId,
                            Program = formFOIPViewModel.Program,
                            Other = formFOIPViewModel.Other,
                            StuSign = formFOIPViewModel.StuSign,
                            StuSignDate = formFOIPViewModel.StuSignDate,
                            Acknowledged = formFOIPViewModel.Acknowledged,
                            Submitted = true,
                        }
                    },
                    StudentId = stuId
                };
                _practicumFormsRepository.Add(placement.PracticumForms.FormFOIP);
                _practicumFormsRepository.Add(placement.PracticumForms);
                _placementRepository.Add(placement);
                return RedirectToAction("Index");
            }
            return View(formFOIPViewModel);
        }

        // Form StuInfo submission handler
        public async Task<IActionResult> CreateFormStuInfo()
        {
            if (User.IsInRole("student"))
            {
                var usr = await _userManager.GetUserAsync(User);
                int stuId = Convert.ToInt32(usr.StudentId);
                var student = await _studentRepository.GetByIdAsync(stuId);
                var usrLastName = student.LastName;
                var usrFirstName = student.FirstName;
                var usrStuId = student.StuId;
                var usrEmail = student.StuEmail;

                var createFormStuInfoViewModel = new CreateFormStuInfoViewModel()
                {
                    StuLastName = usrLastName,
                    StuFirstName = usrFirstName,
                    StuId = usrStuId,
                    CollegeEmail = usrEmail

                };
                return View(createFormStuInfoViewModel);
            }
            else
            {
                var createFormStuInfoViewModel = new CreateFormStuInfoViewModel();
                return View(createFormStuInfoViewModel);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateFormStuInfo(CreateFormStuInfoViewModel formStuInfoViewModel)
        {
            var usr = await _userManager.GetUserAsync(User);
            var placements = await _placementRepository.GetAll();
            var practicumForms = await _practicumFormsRepository.GetAllForms();

            if (User.IsInRole("student"))
            {
                int stuId = Convert.ToInt32(usr.StudentId);

                foreach(var placement in placements )
                {
                    if (placement.StudentId ==  stuId)
                    {
                        foreach(var practicumForm in practicumForms)
                        {
                            if (placement.PracticumFormsId == practicumForm.Id)
                            {
                                 if (ModelState.IsValid)
                                    {
                                practicumForm.FormStuInfo = new FormStuInfo()
                                {
                                    StuLastName = formStuInfoViewModel.StuLastName,
                                    StuFirstName = formStuInfoViewModel.StuFirstName,
                                    StuId = formStuInfoViewModel.StuId,
                                    Program = formStuInfoViewModel.Program,
                                    ProgStartDate = formStuInfoViewModel.ProgStartDate,
                                    PracStartDate = formStuInfoViewModel.PracStartDate,
                                    CollegeEmail = formStuInfoViewModel.CollegeEmail,
                                    PhoneNumber = formStuInfoViewModel.PhoneNumber,
                                    AltPhoneNumber = formStuInfoViewModel.AltPhoneNumber,
                                    Address = new Address()
                                    {
                                        Street = formStuInfoViewModel.CreateAddressViewModel.Street,
                                        City = formStuInfoViewModel.CreateAddressViewModel.City,
                                        Prov = formStuInfoViewModel.CreateAddressViewModel.Prov,
                                        Country = formStuInfoViewModel.CreateAddressViewModel.Country,
                                        PostalCode = formStuInfoViewModel.CreateAddressViewModel.PostalCode
                                    },
                                    Submitted = true
                                    };
                                _practicumFormsRepository.Add(practicumForm.FormStuInfo);
                                return RedirectToAction("Index");
                                }
                            }
                                  
                        }
                        
                    }
                }
            }
            return View(formStuInfoViewModel);
        }

        public async Task<IActionResult> SearchEmployer()
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
       
        // Form A submission handler
        public async Task<IActionResult> CreateFormA(int id)
        {
            Employer employer = await _employerRepository.GetByIdAsync(id);

            if (User.IsInRole("student"))
            {
                var usr = await _userManager.GetUserAsync(User);
                int stuId = Convert.ToInt32(usr.StudentId);
                var student = await _studentRepository.GetByIdAsync(stuId);
                var usrLastName = student.LastName;
                var usrFirstName = student.FirstName;
                var usrStuId = student.StuId;

                if (employer != null)
                {
                    int empAddressId = Convert.ToInt32(employer.AddressId);
                    var empAddress = await _addressRepository.GetByIdAsync(empAddressId);
                    var createFormAViewModel = new CreateFormAViewModel()
                    {
                        StuLastName = usrLastName,
                        StuFirstName = usrFirstName,
                        StuId = usrStuId,
                        StartDate = DateTime.Now,
                        HostCompany = employer.CompanyName,
                        OrgType = employer.OrgType,
                        SVFirstName = employer.FirstName,
                        SVLastName = employer.LastName,
                        SVPosition = employer.SVPosition,
                        SVEmail = employer.EmpEmail,
                        SVPhoneNumber = employer.PhoneNumber,
                        SVCredentials = employer.Credentials,
                        SVCredOther = employer.CredOther,
                        CreateAddressViewModel = new CreateAddressViewModel()
                        {
                            Street = empAddress.Street,
                            City = empAddress.City,
                            Prov = empAddress.Prov,
                            Country = empAddress.Country,
                            PostalCode = empAddress.PostalCode
                        }
                    // database created. create a new employer
                    };
                    return View(createFormAViewModel);
                }
                else
                {
                    var createFormAViewModel = new CreateFormAViewModel()
                    {
                        StuLastName = usrLastName,
                        StuFirstName = usrFirstName,
                        StuId = usrStuId,
                        StartDate = DateTime.Now,
                    };
                    return View(createFormAViewModel);
                }
                
            }
            else
            {
                var createFormAViewModel = new CreateFormAViewModel();
                return View(createFormAViewModel);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateFormA(CreateFormAViewModel formAViewModel, List<string> credentialsList)
        {
            string credentials = string.Join(", ", credentialsList);

            if (credentials != "")
            {
                ModelState.SetModelValue("SVCredentials", new ValueProviderResult(credentials, CultureInfo.InvariantCulture));
                formAViewModel.SVCredentials = credentials;
                ModelState.Remove("SVCredentials");
            }

            var usr = await _userManager.GetUserAsync(User);
            var placements = await _placementRepository.GetAll();
            var practicumForms = await _practicumFormsRepository.GetAllForms();

            if (User.IsInRole("student"))
            {
                int stuId = Convert.ToInt32(usr.StudentId);

                foreach (var placement in placements)
                {
                    
                    if (placement.StudentId == stuId)
                    {
                        var curEmployer = await _employerRepository.GetByEmailAsync(formAViewModel.SVEmail);
                        placement.Employer = curEmployer;
                        foreach (var practicumForm in practicumForms)
                        {
                            if (placement.PracticumFormsId == practicumForm.Id)
                            {

                                if (ModelState.IsValid)
                                {
                                    practicumForm.FormA = new FormA()
                                    {
                                        StuLastName = formAViewModel.StuLastName,
                                        StuFirstName = formAViewModel.StuFirstName,
                                        StuId = formAViewModel.StuId,
                                        Program = formAViewModel.Program,
                                        HostCompany = formAViewModel.StuLastName,
                                        OrgType = formAViewModel.OrgType,
                                        SVFirstName = formAViewModel.SVFirstName,
                                        SVLastName = formAViewModel.SVLastName,
                                        SVPosition = formAViewModel.SVPosition,
                                        SVEmail = formAViewModel.SVEmail,
                                        SVPhoneNumber = formAViewModel.SVPhoneNumber,
                                        SVCredentials = formAViewModel.SVCredentials,
                                        SVCredOther = formAViewModel.SVCredOther,
                                        Address = new Address()
                                        {
                                            Street = formAViewModel.CreateAddressViewModel.Street,
                                            City = formAViewModel.CreateAddressViewModel.City,
                                            Prov = formAViewModel.CreateAddressViewModel.Prov,
                                            Country = formAViewModel.CreateAddressViewModel.Country,
                                            PostalCode = formAViewModel.CreateAddressViewModel.PostalCode,
                                        },
                                        StartDate = formAViewModel.StartDate,
                                        PaymentCategory = formAViewModel.PaymentCategory,
                                        OutOfCountry = formAViewModel.OutOfCountry,
                                        Submitted = true
                                    };
                                    var employer = _employerRepository.GetByEmailAsync(formAViewModel.SVEmail);

                                    _placementRepository.Update(placement);
                                    _practicumFormsRepository.Add(practicumForm.FormA);
                                    
                                    return RedirectToAction("Index");
                                }
                            }
                        }
                    }
                }
            }
            return View(formAViewModel);
        }
        // Form B submission handler
        public async Task<IActionResult> CreateFormB()
        {
            if (User.IsInRole("student"))
            {
                var usr = await _userManager.GetUserAsync(User);
                int stuId = Convert.ToInt32(usr.StudentId);
                var student = await _studentRepository.GetByIdAsync(stuId);
                var usrLastName = student.LastName;
                var usrFirstName = student.FirstName;
                var usrStuId = student.StuId;

                var createFormBViewModel = new CreateFormBViewModel()
                {
                    StuName = usrFirstName + " " + usrLastName,
                };
                return View(createFormBViewModel);
            }
            else
            {
                var createFormBViewModel = new CreateFormBViewModel();
                return View(createFormBViewModel);
            }
        }


        // Form C submission handler
        public IActionResult FormCSubmit()
        {

            var submitFormCVM = new PracticumForms();

            return View(submitFormCVM);

        }
        [HttpPost]
        public async Task<IActionResult> FormCSubmit(PracticumForms submitFormCVM)
        {
            if (ModelState.IsValid)
            {
                var formC = new FormC()
                {
                    PracSV = submitFormCVM.FormC.PracSV,
                    Org = submitFormCVM.FormC.Org,
                    StuName = submitFormCVM.FormC.StuName,
                    A1 = submitFormCVM.FormC.A1,
                    A2 = submitFormCVM.FormC.A2,
                    A3 = submitFormCVM.FormC.A3,
                    A4 = submitFormCVM.FormC.A4,
                    A5 = submitFormCVM.FormC.A5,
                    AComments = submitFormCVM.FormC.AComments,
                    B1 = submitFormCVM.FormC.B1,
                    B2 = submitFormCVM.FormC.B2,
                    B3 = submitFormCVM.FormC.B3,
                    B4 = submitFormCVM.FormC.B4,
                    B5 = submitFormCVM.FormC.B5,
                    B6 = submitFormCVM.FormC.B6,
                    B7 = submitFormCVM.FormC.B7,
                    B8 = submitFormCVM.FormC.B8,
                    BComments = submitFormCVM.FormC.BComments,
                    C1 = submitFormCVM.FormC.C1,
                    C2 = submitFormCVM.FormC.C2,
                    C3 = submitFormCVM.FormC.C3,
                    C4 = submitFormCVM.FormC.C4,
                    C5 = submitFormCVM.FormC.C5,
                    C6 = submitFormCVM.FormC.C6,
                    C7 = submitFormCVM.FormC.C7,
                    C8 = submitFormCVM.FormC.C8,
                    C9 = submitFormCVM.FormC.C9,
                    C10 = submitFormCVM.FormC.C10,
                    C11 = submitFormCVM.FormC.C11,
                    C12 = submitFormCVM.FormC.C12,
                    CComments = submitFormCVM.FormC.CComments,
                    PracSVComments = submitFormCVM.FormC.PracSVComments,
                    SVSign = submitFormCVM.FormC.SVSign,
                    SVSubmitted = submitFormCVM.FormC.SVSubmitted,
                    StuComments = submitFormCVM.FormC.StuComments,
                    StuSign = submitFormCVM.FormC.StuSign,
                    StuSubmitted = submitFormCVM.FormC.StuSubmitted,
                    InsComments = submitFormCVM.FormC.InsComments,
                    InsSign = submitFormCVM.FormC.InsSign,
                    InsSubmitted = submitFormCVM.FormC.InsSubmitted
                };
                _practicumFormsRepository.Add(formC);
                return RedirectToAction("Index");
            }
            return View(submitFormCVM);
        }
        // Form D submission handler
        public IActionResult FormDSubmit()
        {
            var submitFormDVM = new PracticumForms();

            return View(submitFormDVM);
        }
        [HttpPost]
        public async Task<IActionResult> FormDSubmit(PracticumForms submitFormDVM)
        {
            if (ModelState.IsValid)
            {
                var formD = new FormD()
                {
                    PracSV = submitFormDVM.FormD.PracSV,
                    Org = submitFormDVM.FormD.Org,
                    StuName = submitFormDVM.FormD.StuName,
                    A1 = submitFormDVM.FormD.A1,
                    A2 = submitFormDVM.FormD.A2,
                    A3 = submitFormDVM.FormD.A3,
                    A4 = submitFormDVM.FormD.A4,
                    A5 = submitFormDVM.FormD.A5,
                    AComments = submitFormDVM.FormD.AComments,
                    B1 = submitFormDVM.FormD.B1,
                    B2 = submitFormDVM.FormD.B2,
                    B3 = submitFormDVM.FormD.B3,
                    B4 = submitFormDVM.FormD.B4,
                    B5 = submitFormDVM.FormD.B5,
                    B6 = submitFormDVM.FormD.B6,
                    B7 = submitFormDVM.FormD.B7,
                    B8 = submitFormDVM.FormD.B8,
                    BComments = submitFormDVM.FormD.BComments,
                    C1 = submitFormDVM.FormD.C1,
                    C2 = submitFormDVM.FormD.C2,
                    C3 = submitFormDVM.FormD.C3,
                    C4 = submitFormDVM.FormD.C4,
                    C5 = submitFormDVM.FormD.C5,
                    C6 = submitFormDVM.FormD.C6,
                    C7 = submitFormDVM.FormD.C7,
                    C8 = submitFormDVM.FormD.C8,
                    C9 = submitFormDVM.FormD.C9,
                    C10 = submitFormDVM.FormD.C10,
                    C11 = submitFormDVM.FormD.C11,
                    C12 = submitFormDVM.FormD.C12,
                    CComments = submitFormDVM.FormD.CComments,
                    PracSVComments = submitFormDVM.FormD.PracSVComments,
                    SVSign = submitFormDVM.FormD.SVSign,
                    SVSubmitted = submitFormDVM.FormD.SVSubmitted,
                    StuComments = submitFormDVM.FormD.StuComments,
                    StuSign = submitFormDVM.FormD.StuSign,
                    StuSubmitted = submitFormDVM.FormD.StuSubmitted,
                    InsComments = submitFormDVM.FormD.InsComments,
                    InsSign = submitFormDVM.FormD.InsSign,
                    InsSubmitted = submitFormDVM.FormD.InsSubmitted
                };
                _practicumFormsRepository.Add(formD);
                return RedirectToAction("Index");
            }
            return View(submitFormDVM);
        }

       
    }
}
