
using CITPracticum.Data;
using CITPracticum.Data.Migrations;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient.Server;
using System.Globalization;
using System.Net;


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
            var placements = await _placementRepository.GetAll();
          

            if (User.IsInRole("student"))
            {
                var usr = await _userManager.GetUserAsync(User);
                int stuId = Convert.ToInt32(usr.StudentId);
                var student = await _studentRepository.GetByIdAsync(stuId);

                foreach (var placement in placements)
                {
                    if (placement.StudentId == stuId)
                    {
                        var employerId = Convert.ToInt32(placement.EmployerId);
                        var employer = await _employerRepository.GetByIdAsync(employerId);
                        var practicumFormId = Convert.ToInt32(placement.PracticumFormsId);
                        var practicumForm = await _practicumFormsRepository.FormsGetByIdAsync(practicumFormId);
                        var addressId = Convert.ToInt32(employer.AddressId);
                        var address = await _addressRepository.GetByIdAsync(addressId);
                        var formBs = await _practicumFormsRepository.GetAllFormB();
                        var stuSignature = "";
                        var stuSignatureDate = DateTime.Now;
                        var empSignature = "";
                        var empSignatureDate = DateTime.Now;

                        if (practicumForm.FormBId != null)
                        {
                            foreach (var formB in formBs)
                            {
                                if (formB.Id == placement.PracticumForms.FormBId)
                                {
                                    stuSignature = formB.StuSign;
                                    stuSignatureDate = formB.StuSignDate;
                                    empSignature = formB.EmpSign;
                                    empSignatureDate = formB.EmpSignDate;
                                }
                            }
                        }

                        var createFormBViewModel = new CreateFormBViewModel()
                        {
                            StuName = student.FirstName + " " + student.LastName,
                            PracHost = employer.CompanyName,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now,
                            OrgName = employer.CompanyName,
                            PracSV = employer.FirstName + " " + employer.LastName,
                            Address = new Address()
                            {
                                Street = address.Street,
                                City = address.City,
                                Prov = address.Prov,
                                Country = address.Country,
                                PostalCode = address.PostalCode
                            },
                            Position = employer.SVPosition,
                            Email = employer.EmpEmail,
                            Phone = employer.PhoneNumber,
                            StuSign = stuSignature,
                           StuSignDate = stuSignatureDate,
                           EmpSign = empSignature,
                           EmpSignDate = empSignatureDate
                         
                        }; // stopped here enter form B on forms database
                        return View(createFormBViewModel);
                    }
                    
                }
                
            }
            else if (User.IsInRole("employer"))
            {
                var usr = await _userManager.GetUserAsync(User);
                int empId = Convert.ToInt32(usr.EmployerId);
                var employer = await _employerRepository.GetByIdAsync(empId);

                foreach (var placement in placements)
                {
                    if (placement.EmployerId == empId)
                    {
                        var stuId = Convert.ToInt32(placement.StudentId);
                        var student = await _studentRepository.GetByIdAsync(stuId);
                        var practicumFormId = Convert.ToInt32(placement.PracticumFormsId);
                        var practicumForm = await _practicumFormsRepository.FormsGetByIdAsync(practicumFormId);
                        var addressId = Convert.ToInt32(employer.AddressId);
                        var address = await _addressRepository.GetByIdAsync(addressId);
                        var formBs = await _practicumFormsRepository.GetAllFormB();
                        var stuSignature = "";
                        var stuSignatureDate = DateTime.Now;
                        var empSignature = "";
                        var empSignatureDate = DateTime.Now;
                        var pracStarteDate = DateTime.Now;
                        var pracEndDate = DateTime.Now;

                        if (practicumForm.FormBId != null)
                        {
                            foreach (var formB in formBs)
                            {
                                if (formB.Id == placement.PracticumForms.FormBId)
                                {
                                    stuSignature = formB.StuSign;
                                    stuSignatureDate = formB.StuSignDate;
                                    empSignature = formB.EmpSign;
                                    empSignatureDate = formB.EmpSignDate;
                                    pracStarteDate = formB.StartDate;
                                    pracEndDate = formB.EndDate;
                                }
                            }
                        }

                        var createFormBViewModel = new CreateFormBViewModel()
                        {
                            StuName = student.FirstName + " " + student.LastName,
                            PracHost = employer.CompanyName,
                            StartDate = pracStarteDate,
                            EndDate = pracEndDate,
                            OrgName = employer.CompanyName,
                            PracSV = employer.FirstName + " " + employer.LastName,
                            Address = new Address()
                            {
                                Street = address.Street,
                                City = address.City,
                                Prov = address.Prov,
                                Country = address.Country,
                                PostalCode = address.PostalCode
                            },
                            Position = employer.SVPosition,
                            Email = employer.EmpEmail,
                            Phone = employer.PhoneNumber,
                            StuSign = stuSignature,
                            StuSignDate = stuSignatureDate,
                            EmpSign = empSignature,
                            EmpSignDate = empSignatureDate

                        };
                        return View(createFormBViewModel);
                    }
                }
            } 
            else
            {
                var createFormBViewModel = new CreateFormBViewModel();
                return View(createFormBViewModel);
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateFormB(CreateFormBViewModel formBViewModel)
        {
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
                        var practicumFormId = Convert.ToInt32(placement.PracticumFormsId);
                        var practicumForm = await _practicumFormsRepository.FormsGetByIdAsync(practicumFormId);
                        var formBs = await _practicumFormsRepository.GetAllFormB();
                        string studentSignature =  Convert.ToString(formBViewModel.StuSign);
                        string employerSignature = Convert.ToString(formBViewModel.EmpSign);

                        if (ModelState.IsValid)
                        {
                            if (practicumForm.FormBId == null)
                            {
                                practicumForm.FormB = new FormB()
                                {
                                    StuName = formBViewModel.StuName,
                                    PracHost = formBViewModel.PracHost,
                                    StartDate = formBViewModel.StartDate,
                                    EndDate = formBViewModel.EndDate,
                                    OrgName = formBViewModel.OrgName,
                                    PracSV = formBViewModel.PracSV,
                                    Address = new Address()
                                    {
                                        Street = formBViewModel.Address.Street,
                                        City = formBViewModel.Address.City,
                                        Prov = formBViewModel.Address.Prov,
                                        Country = formBViewModel.Address.Country,
                                        PostalCode = formBViewModel.Address.PostalCode
                                    },
                                    Position = formBViewModel.Position,
                                    Email = formBViewModel.Email,
                                    Phone = formBViewModel.Phone,
                                    StuSign = studentSignature,
                                    StuSignDate = formBViewModel.StuSignDate,
                                    EmpSign = employerSignature,
                                    EmpSignDate = formBViewModel.EmpSignDate
                                };
                                _practicumFormsRepository.Add(practicumForm.FormB);
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                foreach (var formB in formBs)
                                {
                                    if (formB.Id == practicumForm.FormBId)
                                    {
                                        formB.StartDate = formBViewModel.StartDate;
                                        formB.EndDate = formBViewModel.EndDate;
                                        formB.StuSign = formBViewModel.StuSign;
                                        formB.StuSignDate = formBViewModel.StuSignDate;
                                        if (formB.StuSign != null && formB.EmpSign != null)
                                        {
                                            formB.Submitted = true;
                                        }
                                    }
                                    _practicumFormsRepository.Update(formB);
                                    return RedirectToAction("Index");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (User.IsInRole("employer"))
                {
                    int empId = Convert.ToInt32(usr.EmployerId);

                    foreach (var placement in placements)
                    {
                        if (placement.EmployerId == empId)
                        {
                            var practicumFormId = Convert.ToInt32(placement.PracticumFormsId);
                            var practicumForm = await _practicumFormsRepository.FormsGetByIdAsync(practicumFormId);
                            var formBs = await _practicumFormsRepository.GetAllFormB();
                            string studentSignature = Convert.ToString(formBViewModel.StuSign);
                            string employerSignature = Convert.ToString(formBViewModel.EmpSign);

                            if (ModelState.IsValid)
                            {
                                if (practicumForm.FormBId == null)
                                {
                                    practicumForm.FormB = new FormB()
                                    {
                                        StuName = formBViewModel.StuName,
                                        PracHost = formBViewModel.PracHost,
                                        StartDate = formBViewModel.StartDate,
                                        EndDate = formBViewModel.EndDate,
                                        OrgName = formBViewModel.OrgName,
                                        PracSV = formBViewModel.PracSV,
                                        Address = new Address()
                                        {
                                            Street = formBViewModel.Address.Street,
                                            City = formBViewModel.Address.City,
                                            Prov = formBViewModel.Address.Prov,
                                            Country = formBViewModel.Address.Country,
                                            PostalCode = formBViewModel.Address.PostalCode
                                        },
                                        Position = formBViewModel.Position,
                                        Email = formBViewModel.Email,
                                        Phone = formBViewModel.Phone,
                                        StuSign = studentSignature,
                                        StuSignDate = formBViewModel.StuSignDate,
                                        EmpSign = employerSignature,
                                        EmpSignDate = formBViewModel.EmpSignDate
                                    };
                                    _practicumFormsRepository.Add(practicumForm.FormB);
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    foreach (var formB in formBs)
                                    {
                                        if (formB.Id == practicumForm.FormBId)
                                        {
                                            formB.StartDate = formBViewModel.StartDate;
                                            formB.EndDate = formBViewModel.EndDate;
                                            formB.EmpSign = formBViewModel.EmpSign;
                                            formB.EmpSignDate = formBViewModel.EmpSignDate;
                                            if (formB.StuSign != null && formB.EmpSign != null)
                                            {
                                                formB.Submitted = true;
                                            }
                                        }
                                        _practicumFormsRepository.Update(formB);
                                        return RedirectToAction("Index");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return View(formBViewModel);
        }


        // Form C submission handler
        public async Task<IActionResult> CreateFormC()
        {

            var placements = await _placementRepository.GetAll();


            if (User.IsInRole("student"))
            {
                var usr = await _userManager.GetUserAsync(User);
                int stuId = Convert.ToInt32(usr.StudentId);
                var student = await _studentRepository.GetByIdAsync(stuId);

                foreach (var placement in placements)
                {
                    if (placement.StudentId == stuId)
                    {
                        var employerId = Convert.ToInt32(placement.EmployerId);
                        var employer = await _employerRepository.GetByIdAsync(employerId);
                        var practicumFormId = Convert.ToInt32(placement.PracticumFormsId);
                        var practicumForm = await _practicumFormsRepository.FormsGetByIdAsync(practicumFormId);
                        var formCs = await _practicumFormsRepository.GetAllFormC();

                        if (practicumForm.FormCId != null)
                        {
                            foreach (var formC in formCs)
                            {
                                if (formC.Id == placement.PracticumForms.FormCId)
                                {
                                    var createFormCViewModel = new CreateFormCViewModel()
                                    {
                                        PracSV = formC.PracSV,
                                        Org = formC.Org,
                                        StuName = formC.StuName,
                                        A1 = formC.A1,
                                        A2 = formC.A2,
                                        A3 = formC.A3,
                                        A4 = formC.A4,
                                        A5 = formC.A5,
                                        AComments = formC.AComments,
                                        B1 = formC.B1,
                                        B2 = formC.B2,
                                        B3 = formC.B3,
                                        B4 = formC.B4,
                                        B5 = formC.B5,
                                        B6 = formC.B6,
                                        B7 = formC.B7,
                                        B8 = formC.B8,
                                        BComments = formC.BComments,
                                        C1 = formC.C1,
                                        C2 = formC.C2,
                                        C3 = formC.C3,
                                        C4 = formC.C4,
                                        C5 = formC.C5,
                                        C6 = formC.C6,
                                        C7 = formC.C7,
                                        C8 = formC.C8,
                                        C9 = formC.C9,
                                        C10 = formC.C10,
                                        C11 = formC.C11,
                                        C12 = formC.C12,
                                        CComments = formC.CComments,
                                        PracSVComments = formC.PracSVComments,
                                        SVSign = formC.SVSign,
                                        StuComments = formC.StuComments,
                                        StuSign = formC.StuSign,
                                        InsComments = formC.InsComments,
                                        InsSign = formC.InsSign
                                    }; // stopped here enter form B on forms database
                                    return View(createFormCViewModel);
                                }
                            }
                        }
                        else
                        {
                            var createFormCViewModel = new CreateFormCViewModel()
                            {
                                PracSV = employer.FirstName + " " + employer.LastName,
                                Org = employer.CompanyName,
                                StuName = student.LastName + " " + student.LastName,

                            };
                            return View(createFormCViewModel);
                        }


                    }

                }

            }
            else if (User.IsInRole("employer"))
            {
                var usr = await _userManager.GetUserAsync(User);
                int empId = Convert.ToInt32(usr.EmployerId);
                var employer = await _employerRepository.GetByIdAsync(empId);

                foreach (var placement in placements)
                {
                    if (placement.EmployerId == empId)
                    {
                        var stuId = Convert.ToInt32(placement.StudentId);
                        var student = await _studentRepository.GetByIdAsync(stuId);
                        var practicumFormId = Convert.ToInt32(placement.PracticumFormsId);
                        var practicumForm = await _practicumFormsRepository.FormsGetByIdAsync(practicumFormId);
                        var formCs = await _practicumFormsRepository.GetAllFormC();

                        if (practicumForm.FormCId != null)
                        {
                            foreach (var formC in formCs)
                            {
                                if (formC.Id == placement.PracticumForms.FormCId)
                                {
                                    var createFormCViewModel = new CreateFormCViewModel()
                                    {
                                        PracSV = formC.PracSV,
                                        Org = formC.Org,
                                        StuName = formC.StuName,
                                        A1 = formC.A1,
                                        A2 = formC.A2,
                                        A3 = formC.A3,
                                        A4 = formC.A4,
                                        A5 = formC.A5,
                                        AComments = formC.AComments,
                                        B1 = formC.B1,
                                        B2 = formC.B2,
                                        B3 = formC.B3,
                                        B4 = formC.B4,
                                        B5 = formC.B5,
                                        B6 = formC.B6,
                                        B7 = formC.B7,
                                        B8 = formC.B8,
                                        BComments = formC.BComments,
                                        C1 = formC.C1,
                                        C2 = formC.C2,
                                        C3 = formC.C3,
                                        C4 = formC.C4,
                                        C5 = formC.C5,
                                        C6 = formC.C6,
                                        C7 = formC.C7,
                                        C8 = formC.C8,
                                        C9 = formC.C9,
                                        C10 = formC.C10,
                                        C11 = formC.C11,
                                        C12 = formC.C12,
                                        CComments = formC.CComments,
                                        PracSVComments = formC.PracSVComments,
                                        SVSign = formC.SVSign,
                                        StuComments = formC.StuComments,
                                        StuSign = formC.StuSign,
                                        InsComments = formC.InsComments,
                                        InsSign = formC.InsSign
                                    }; // stopped here enter form B on forms database
                                    return View(createFormCViewModel);
                                }
                            }
                        }
                        else
                        {
                            var createFormCViewModel = new CreateFormCViewModel()
                            {
                                PracSV = employer.FirstName + " " + employer.LastName,
                                Org = employer.CompanyName,
                                StuName = student.FirstName + " " + student.LastName,

                            };
                            return View(createFormCViewModel);
                        }
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateFormC(CreateFormCViewModel formCViewModel)
        {
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
                        var practicumFormId = Convert.ToInt32(placement.PracticumFormsId);
                        var practicumForm = await _practicumFormsRepository.FormsGetByIdAsync(practicumFormId);
                        var formCs = await _practicumFormsRepository.GetAllFormC();
                        string studentSignature = Convert.ToString(formCViewModel.StuSign);
                        string employerSignature = Convert.ToString(formCViewModel.SVSign);

                        if (ModelState.IsValid)
                        {
                            if (practicumForm.FormCId == null)
                            {
                                practicumForm.FormC = new FormC()
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
                                    StuComments = formCViewModel.StuComments,
                                    StuSign = formCViewModel.StuSign,
                                    StuSubmitted = true,
                                    InsComments = formCViewModel.InsComments,
                                    InsSign = formCViewModel.InsSign,
                                };
                                _practicumFormsRepository.Add(practicumForm.FormC);
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                foreach (var formC in formCs)
                                {
                                    if (formC.Id == practicumForm.FormCId)
                                    {
                                        formC.StuComments = formCViewModel.StuComments;
                                        formC.StuSign = formCViewModel.StuSign;
                                        formC.StuSubmitted = true;
                                    }
                                    _practicumFormsRepository.Update(formC);
                                    return RedirectToAction("Index");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (User.IsInRole("employer"))
                {
                    int empId = Convert.ToInt32(usr.EmployerId);

                    foreach (var placement in placements)
                    {
                        if (placement.EmployerId == empId)
                        {
                            var practicumFormId = Convert.ToInt32(placement.PracticumFormsId);
                            var practicumForm = await _practicumFormsRepository.FormsGetByIdAsync(practicumFormId);
                            var formCs = await _practicumFormsRepository.GetAllFormC();
                            string studentSignature = Convert.ToString(formCViewModel.StuSign);
                            string employerSignature = Convert.ToString(formCViewModel.SVSign);

                            if (ModelState.IsValid)
                            {
                                if (practicumForm.FormCId == null)
                                {
                                    practicumForm.FormC = new FormC()
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
                                        StuComments = formCViewModel.StuComments,
                                        StuSign = formCViewModel.StuSign,
                                        SVSubmitted = true,
                                        InsComments = formCViewModel.InsComments,
                                        InsSign = formCViewModel.InsSign,
                                    };
                                    _practicumFormsRepository.Add(practicumForm.FormC);
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    foreach (var formC in formCs)
                                    {
                                        if (formC.Id == practicumForm.FormCId)
                                        {
                                            formC.PracSV = formCViewModel.PracSV;
                                            formC.Org = formCViewModel.Org;
                                            formC.StuName = formCViewModel.StuName;
                                            formC.A1 = formCViewModel.A1;
                                            formC.A2 = formCViewModel.A2;
                                            formC.A3 = formCViewModel.A3;
                                            formC.A4 = formCViewModel.A4;
                                            formC.A5 = formCViewModel.A5;
                                            formC.AComments = formCViewModel.AComments;
                                            formC.B1 = formCViewModel.B1;
                                            formC.B2 = formCViewModel.B2;
                                            formC.B3 = formCViewModel.B3;
                                            formC.B4 = formCViewModel.B4;
                                            formC.B5 = formCViewModel.B5;
                                            formC.B6 = formCViewModel.B6;
                                            formC.B7 = formCViewModel.B7;
                                            formC.B8 = formCViewModel.B8;
                                            formC.BComments = formCViewModel.BComments;
                                            formC.C1 = formCViewModel.C1;
                                            formC.C2 = formCViewModel.C2;
                                            formC.C3 = formCViewModel.C3;
                                            formC.C4 = formCViewModel.C4;
                                            formC.C5 = formCViewModel.C5;
                                            formC.C6 = formCViewModel.C6;
                                            formC.C7 = formCViewModel.C7;
                                            formC.C8 = formCViewModel.C8;
                                            formC.C9 = formCViewModel.C9;
                                            formC.C10 = formCViewModel.C10;
                                            formC.C11 = formCViewModel.C11;
                                            formC.C12 = formCViewModel.C12;
                                            formC.CComments = formCViewModel.CComments;
                                            formC.PracSVComments = formCViewModel.PracSVComments;
                                            formC.SVSign = formCViewModel.SVSign;
                                            formC.StuComments = formCViewModel.StuComments;
                                            formC.StuSign = formCViewModel.StuSign;
                                            formC.SVSubmitted = true;
                                            formC.InsComments = formCViewModel.InsComments;
                                            formC.InsSign = formCViewModel.InsSign;
                                        }
                                        _practicumFormsRepository.Update(formC);
                                        return RedirectToAction("Index");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return View(formCViewModel);      
        }
        // Form D submission handler
        public async Task<IActionResult> CreateFormD()
        {
            var placements = await _placementRepository.GetAll();


            if (User.IsInRole("student"))
            {
                var usr = await _userManager.GetUserAsync(User);
                int stuId = Convert.ToInt32(usr.StudentId);
                var student = await _studentRepository.GetByIdAsync(stuId);

                foreach (var placement in placements)
                {
                    if (placement.StudentId == stuId)
                    {
                        var employerId = Convert.ToInt32(placement.EmployerId);
                        var employer = await _employerRepository.GetByIdAsync(employerId);
                        var practicumFormId = Convert.ToInt32(placement.PracticumFormsId);
                        var practicumForm = await _practicumFormsRepository.FormsGetByIdAsync(practicumFormId);
                        var formDs = await _practicumFormsRepository.GetAllFormD();

                        if (practicumForm.FormDId != null)
                        {
                            foreach (var formD in formDs)
                            {
                                if (formD.Id == placement.PracticumForms.FormDId)
                                {
                                    var createFormDViewModel = new CreateFormDViewModel()
                                    {
                                        PracSV = formD.PracSV,
                                        Org = formD.Org,
                                        StuName = formD.StuName,
                                        A1 = formD.A1,
                                        A2 = formD.A2,
                                        A3 = formD.A3,
                                        A4 = formD.A4,
                                        A5 = formD.A5,
                                        AComments = formD.AComments,
                                        B1 = formD.B1,
                                        B2 = formD.B2,
                                        B3 = formD.B3,
                                        B4 = formD.B4,
                                        B5 = formD.B5,
                                        B6 = formD.B6,
                                        B7 = formD.B7,
                                        B8 = formD.B8,
                                        BComments = formD.BComments,
                                        C1 = formD.C1,
                                        C2 = formD.C2,
                                        C3 = formD.C3,
                                        C4 = formD.C4,
                                        C5 = formD.C5,
                                        C6 = formD.C6,
                                        C7 = formD.C7,
                                        C8 = formD.C8,
                                        C9 = formD.C9,
                                        C10 = formD.C10,
                                        C11 = formD.C11,
                                        C12 = formD.C12,
                                        CComments = formD.CComments,
                                        PracSVComments = formD.PracSVComments,
                                        SVSign = formD.SVSign,
                                        StuComments = formD.StuComments,
                                        StuSign = formD.StuSign,
                                        InsComments = formD.InsComments,
                                        InsSign = formD.InsSign
                                    }; // stopped here enter form B on forms database
                                    return View(createFormDViewModel);
                                }
                            }
                        }
                        else
                        {
                            var createFormDViewModel = new CreateFormDViewModel()
                            {
                                PracSV = employer.FirstName + " " + employer.LastName,
                                Org = employer.CompanyName,
                                StuName = student.LastName + " " + student.LastName,

                            };
                            return View(createFormDViewModel);
                        }


                    }

                }

            }
            else if (User.IsInRole("employer"))
            {
                var usr = await _userManager.GetUserAsync(User);
                int empId = Convert.ToInt32(usr.EmployerId);
                var employer = await _employerRepository.GetByIdAsync(empId);

                foreach (var placement in placements)
                {
                    if (placement.EmployerId == empId)
                    {
                        var stuId = Convert.ToInt32(placement.StudentId);
                        var student = await _studentRepository.GetByIdAsync(stuId);
                        var practicumFormId = Convert.ToInt32(placement.PracticumFormsId);
                        var practicumForm = await _practicumFormsRepository.FormsGetByIdAsync(practicumFormId);
                        var formDs = await _practicumFormsRepository.GetAllFormD();

                        if (practicumForm.FormDId != null)
                        {
                            foreach (var formD in formDs)
                            {
                                if (formD.Id == placement.PracticumForms.FormDId)
                                {
                                    var createFormDViewModel = new CreateFormDViewModel()
                                    {
                                        PracSV = formD.PracSV,
                                        Org = formD.Org,
                                        StuName = formD.StuName,
                                        A1 = formD.A1,
                                        A2 = formD.A2,
                                        A3 = formD.A3,
                                        A4 = formD.A4,
                                        A5 = formD.A5,
                                        AComments = formD.AComments,
                                        B1 = formD.B1,
                                        B2 = formD.B2,
                                        B3 = formD.B3,
                                        B4 = formD.B4,
                                        B5 = formD.B5,
                                        B6 = formD.B6,
                                        B7 = formD.B7,
                                        B8 = formD.B8,
                                        BComments = formD.BComments,
                                        C1 = formD.C1,
                                        C2 = formD.C2,
                                        C3 = formD.C3,
                                        C4 = formD.C4,
                                        C5 = formD.C5,
                                        C6 = formD.C6,
                                        C7 = formD.C7,
                                        C8 = formD.C8,
                                        C9 = formD.C9,
                                        C10 = formD.C10,
                                        C11 = formD.C11,
                                        C12 = formD.C12,
                                        CComments = formD.CComments,
                                        PracSVComments = formD.PracSVComments,
                                        SVSign = formD.SVSign,
                                        StuComments = formD.StuComments,
                                        StuSign = formD.StuSign,
                                        InsComments = formD.InsComments,
                                        InsSign = formD.InsSign
                                    }; // stopped here enter form B on forms database
                                    return View(createFormDViewModel);
                                }
                            }
                        }
                        else
                        {
                            var createFormDViewModel = new CreateFormDViewModel()
                            {
                                PracSV = employer.FirstName + " " + employer.LastName,
                                Org = employer.CompanyName,
                                StuName = student.FirstName + " " + student.LastName,

                            };
                            return View(createFormDViewModel);
                        }
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateFormD(CreateFormDViewModel formDViewModel)
        {
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
                        var practicumFormId = Convert.ToInt32(placement.PracticumFormsId);
                        var practicumForm = await _practicumFormsRepository.FormsGetByIdAsync(practicumFormId);
                        var formDs = await _practicumFormsRepository.GetAllFormD();
                        string studentSignature = Convert.ToString(formDViewModel.StuSign);
                        string employerSignature = Convert.ToString(formDViewModel.SVSign);

                        if (ModelState.IsValid)
                        {
                            if (practicumForm.FormDId == null)
                            {
                                practicumForm.FormD = new FormD()
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
                                    StuComments = formDViewModel.StuComments,
                                    StuSign = formDViewModel.StuSign,
                                    StuSubmitted = true,
                                    InsComments = formDViewModel.InsComments,
                                    InsSign = formDViewModel.InsSign,
                                };
                                _practicumFormsRepository.Add(practicumForm.FormD);
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                foreach (var formD in formDs)
                                {
                                    if (formD.Id == practicumForm.FormDId)
                                    {
                                        formD.StuComments = formDViewModel.StuComments;
                                        formD.StuSign = formDViewModel.StuSign;
                                        formD.StuSubmitted = true;
                                    }
                                    _practicumFormsRepository.Update(formD);
                                    return RedirectToAction("Index");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (User.IsInRole("employer"))
                {
                    int empId = Convert.ToInt32(usr.EmployerId);

                    foreach (var placement in placements)
                    {
                        if (placement.EmployerId == empId)
                        {
                            var practicumFormId = Convert.ToInt32(placement.PracticumFormsId);
                            var practicumForm = await _practicumFormsRepository.FormsGetByIdAsync(practicumFormId);
                            var formDs = await _practicumFormsRepository.GetAllFormD();
                            string studentSignature = Convert.ToString(formDViewModel.StuSign);
                            string employerSignature = Convert.ToString(formDViewModel.SVSign);

                            if (ModelState.IsValid)
                            {
                                if (practicumForm.FormDId == null)
                                {
                                    practicumForm.FormD = new FormD()
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
                                        StuComments = formDViewModel.StuComments,
                                        StuSign = formDViewModel.StuSign,
                                        SVSubmitted = true,
                                        InsComments = formDViewModel.InsComments,
                                        InsSign = formDViewModel.InsSign,
                                    };
                                    _practicumFormsRepository.Add(practicumForm.FormD);
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    foreach (var formD in formDs)
                                    {
                                        if (formD.Id == practicumForm.FormDId)
                                        {
                                            formD.PracSV = formDViewModel.PracSV;
                                            formD.Org = formDViewModel.Org;
                                            formD.StuName = formDViewModel.StuName;
                                            formD.A1 = formDViewModel.A1;
                                            formD.A2 = formDViewModel.A2;
                                            formD.A3 = formDViewModel.A3;
                                            formD.A4 = formDViewModel.A4;
                                            formD.A5 = formDViewModel.A5;
                                            formD.AComments = formDViewModel.AComments;
                                            formD.B1 = formDViewModel.B1;
                                            formD.B2 = formDViewModel.B2;
                                            formD.B3 = formDViewModel.B3;
                                            formD.B4 = formDViewModel.B4;
                                            formD.B5 = formDViewModel.B5;
                                            formD.B6 = formDViewModel.B6;
                                            formD.B7 = formDViewModel.B7;
                                            formD.B8 = formDViewModel.B8;
                                            formD.BComments = formDViewModel.BComments;
                                            formD.C1 = formDViewModel.C1;
                                            formD.C2 = formDViewModel.C2;
                                            formD.C3 = formDViewModel.C3;
                                            formD.C4 = formDViewModel.C4;
                                            formD.C5 = formDViewModel.C5;
                                            formD.C6 = formDViewModel.C6;
                                            formD.C7 = formDViewModel.C7;
                                            formD.C8 = formDViewModel.C8;
                                            formD.C9 = formDViewModel.C9;
                                            formD.C10 = formDViewModel.C10;
                                            formD.C11 = formDViewModel.C11;
                                            formD.C12 = formDViewModel.C12;
                                            formD.CComments = formDViewModel.CComments;
                                            formD.PracSVComments = formDViewModel.PracSVComments;
                                            formD.SVSign = formDViewModel.SVSign;
                                            formD.StuComments = formDViewModel.StuComments;
                                            formD.StuSign = formDViewModel.StuSign;
                                            formD.SVSubmitted = true;
                                            formD.InsComments = formDViewModel.InsComments;
                                            formD.InsSign = formDViewModel.InsSign;
                                        }
                                        _practicumFormsRepository.Update(formD);
                                        return RedirectToAction("Index");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return View(formDViewModel);
        }

       
    }
}
