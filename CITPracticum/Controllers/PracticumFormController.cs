
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
                var submitFormAVM = new Placement();
                return View(submitFormAVM);
         
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
                var usrEmail = student.StuEmail;


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
        [HttpPost]
        public async Task<IActionResult> CreateFormB(CreateFormBViewModel formBViewModel)
        { 
                return View(formBViewModel);
        }

        // Form C submission handler
        public IActionResult CreateFormC()
        {

            var createFormCViewModel = new CreateFormCViewModel();

            return View(createFormCViewModel);

        }
        [HttpPost]
        public async Task<IActionResult> CreateFormC(CreateFormCViewModel formCViewModel)
        {
            if (ModelState.IsValid)
            {
                var formC = new FormC()
                {
                    PracSV = formCViewModel.PracSV,
                    Org = formCViewModel.Org,
                    StuName = formCViewModel.StuName,
                    A1 = formCViewModel.A1,
                    A2 = formCViewModel.A2,
                    A3 = formCViewModel.A3,
                    A4 = formCViewModel.A4,
                    A5 = formCViewModel.A5,
                    AComments = formCViewModel.AComments,
                    B1 = formCViewModel.B1,
                    B2 = formCViewModel.B2,
                    B3 = formCViewModel.B3,
                    B4 = formCViewModel.B4,
                    B5 = formCViewModel.B5,
                    B6 = formCViewModel.B6,
                    B7 = formCViewModel.B7,
                    B8 = formCViewModel.B8,
                    BComments = formCViewModel.BComments,
                    C1 = formCViewModel.C1,
                    C2 = formCViewModel.C2,
                    C3 = formCViewModel.C3,
                    C4 = formCViewModel.C4,
                    C5 = formCViewModel.C5,
                    C6 = formCViewModel.C6,
                    C7 = formCViewModel.C7,
                    C8 = formCViewModel.C8,
                    C9 = formCViewModel.C9,
                    C10 = formCViewModel.C10,
                    C11 = formCViewModel.C11,
                    C12 = formCViewModel.C12,
                    CComments = formCViewModel.CComments,
                    PracSVComments = formCViewModel.PracSVComments,
                    SVSign = formCViewModel.SVSign,
                    SVSubmitted = formCViewModel.SVSubmitted,
                    StuComments = formCViewModel.StuComments,
                    StuSign = formCViewModel.StuSign,
                    StuSubmitted = formCViewModel.StuSubmitted,
                    InsComments = formCViewModel.InsComments,
                    InsSign = formCViewModel.InsSign,
                    InsSubmitted = formCViewModel.InsSubmitted
                };
                _practicumFormsRepository.Add(formC);
                return RedirectToAction("Index");
            }
            return View(formCViewModel);
        }
        // Form D submission handler
        public IActionResult CreateFormD()
        {
            var createFormDViewModel = new CreateFormDViewModel();

            return View(createFormDViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateFormD(CreateFormDViewModel formDViewModel)
        {
            if (ModelState.IsValid)
            {
                var formD = new FormD()
                {
                    PracSV = formDViewModel.PracSV,
                    Org = formDViewModel.Org,
                    StuName = formDViewModel.StuName,
                    A1 = formDViewModel.A1,
                    A2 = formDViewModel.A2,
                    A3 = formDViewModel.A3,
                    A4 = formDViewModel.A4,
                    A5 = formDViewModel.A5,
                    AComments = formDViewModel.AComments,
                    B1 = formDViewModel.B1,
                    B2 = formDViewModel.B2,
                    B3 = formDViewModel.B3,
                    B4 = formDViewModel.B4,
                    B5 = formDViewModel.B5,
                    B6 = formDViewModel.B6,
                    B7 = formDViewModel.B7,
                    B8 = formDViewModel.B8,
                    BComments = formDViewModel.BComments,
                    C1 = formDViewModel.C1,
                    C2 = formDViewModel.C2,
                    C3 = formDViewModel.C3,
                    C4 = formDViewModel.C4,
                    C5 = formDViewModel.C5,
                    C6 = formDViewModel.C6,
                    C7 = formDViewModel.C7,
                    C8 = formDViewModel.C8,
                    C9 = formDViewModel.C9,
                    C10 = formDViewModel.C10,
                    C11 = formDViewModel.C11,
                    C12 = formDViewModel.C12,
                    CComments = formDViewModel.CComments,
                    PracSVComments = formDViewModel.PracSVComments,
                    SVSign = formDViewModel.SVSign,
                    SVSubmitted = formDViewModel.SVSubmitted,
                    StuComments = formDViewModel.StuComments,
                    StuSign = formDViewModel.StuSign,
                    StuSubmitted = formDViewModel.StuSubmitted,
                    InsComments = formDViewModel.InsComments,
                    InsSign = formDViewModel.InsSign,
                    InsSubmitted = formDViewModel.InsSubmitted
                };
                _practicumFormsRepository.Add(formD);
                return RedirectToAction("Index");
            }
            return View(formDViewModel);
        }

       
    }
}
