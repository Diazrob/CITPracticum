
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class PracticumFormController : Controller
    {
        private readonly IPracticumFormsRepository _practicumFormsRepository;
        private readonly IPlacementRepository _placementRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IStudentRepository _studentRepository;

        public PracticumFormController(IPracticumFormsRepository practicumFormsRepository, IPlacementRepository placementRepository, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IStudentRepository studentRepository)
        {
            _practicumFormsRepository = practicumFormsRepository;
            _placementRepository = placementRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _studentRepository = studentRepository;
        }
        public async Task<IActionResult> Index()
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

                var placementVM = new Placement()
                {
                    Student = new Student()
                    {
                        LastName = usrLastName,
                        FirstName = usrFirstName,
                        StuId = usrStuId,
                        StuEmail = usrEmail
                    }
                };
                return View(placementVM);
            } else
            {
                var submitFormAVM = new Placement();
                return View(submitFormAVM);
            } 
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

        // Form A submission handler
        public IActionResult FormASubmit()
        {
            var submitFormAVM = new Placement()
            {
            };
            
            return View(submitFormAVM);
        }
        [HttpPost]
        public async Task<IActionResult> FormASubmit(Placement submitFormAVM, List<string> credentialsList)
        {
            if (ModelState.IsValid)
            {
                string credentials = string.Join(", ", credentialsList);

                var placement = new Placement() {

                PracticumForms = new PracticumForms()
                {
                    FormA = new FormA()
                    {
                        StuLastName = submitFormAVM.PracticumForms.FormA.StuLastName,
                        StuFirstName = submitFormAVM.PracticumForms.FormA.StuFirstName,
                        StuId = submitFormAVM.PracticumForms.FormA.StuId,
                        Program = submitFormAVM.PracticumForms.FormA.Program,
                        HostCompany = submitFormAVM.PracticumForms.FormA.HostCompany,
                        OrgType = submitFormAVM.PracticumForms.FormA.OrgType,
                        SVName = submitFormAVM.PracticumForms.FormA.SVName,
                        SVPosition = submitFormAVM.PracticumForms.FormA.SVPosition,
                        SVEmail = submitFormAVM.PracticumForms.FormA.SVEmail,
                        SVPhoneNumber = submitFormAVM.PracticumForms.FormA.SVPhoneNumber,
                        SVCredentials = credentials,
                        SVCredOther = submitFormAVM.PracticumForms.FormA.SVCredOther,
                        Address = new Address()
                        {
                            Street = submitFormAVM.PracticumForms.FormA.Address.Street,
                            City = submitFormAVM.PracticumForms.FormA.Address.City,
                            Prov = submitFormAVM.PracticumForms.FormA.Address.Prov,
                            Country = submitFormAVM.PracticumForms.FormA.Address.Country,
                            PostalCode = submitFormAVM.PracticumForms.FormA.Address.PostalCode
                        },
                        StartDate = submitFormAVM.PracticumForms.FormA.StartDate,
                        PaymentCategory = submitFormAVM.PracticumForms.FormA.PaymentCategory,
                        OutOfCountry = submitFormAVM.PracticumForms.FormA.OutOfCountry,
                        Submitted = true
                    }
                }
            };
                _practicumFormsRepository.Add(placement.PracticumForms.FormA);
                _practicumFormsRepository.Add(placement.PracticumForms);
                _placementRepository.Add(placement);
                return RedirectToAction("Index");
            }
            return View(submitFormAVM);
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

        // Form FOIP submission handler
        public IActionResult FormFOIPSubmit()
        {
            var submitFormFOIPVM = new PracticumForms();

            return View(submitFormFOIPVM);
        }
        [HttpPost]
        public async Task<IActionResult> FormFOIPSubmit(PracticumForms submitFormFOIPVM)
        {
            if (ModelState.IsValid)
            {
                var formFOIP = new FormFOIP()
                {
                    StuFirstName = submitFormFOIPVM.FormFOIP.StuFirstName,
                    StuLastName = submitFormFOIPVM.FormFOIP.StuLastName,
                    StuId = submitFormFOIPVM.FormFOIP.StuId,
                    Program = submitFormFOIPVM.FormFOIP.Program,
                    Other = submitFormFOIPVM.FormFOIP.Other,
                    StuSign = submitFormFOIPVM.FormFOIP.StuSign,
                    StuSignDate = submitFormFOIPVM.FormFOIP.StuSignDate,
                    Submitted = true,
                };
                _practicumFormsRepository.Add(formFOIP);
                return RedirectToAction("Index");
            }
            return View(submitFormFOIPVM);
        }

        // Form StuInfo submission handler
        public IActionResult FormStuInfoSubmit()
        {
            var submitFormStuInfoVM = new PracticumForms();

            return View(submitFormStuInfoVM);
        }
        [HttpPost]
        public async Task<IActionResult> FormStuInfoSubmit(PracticumForms submitFormStuInfoVM)
        {
            if (ModelState.IsValid)
            {
                var formStuInfo = new FormStuInfo()
                {
                    StuLastName = submitFormStuInfoVM.FormStuInfo.StuLastName,
                    StuFirstName = submitFormStuInfoVM.FormStuInfo.StuFirstName,
                    StuId = submitFormStuInfoVM.FormStuInfo.StuId,
                    Program = submitFormStuInfoVM.FormStuInfo.Program,
                    ProgStartDate = submitFormStuInfoVM.FormStuInfo.ProgStartDate,
                    PracStartDate = submitFormStuInfoVM.FormStuInfo.PracStartDate,
                    CollegeEmail = submitFormStuInfoVM.FormStuInfo.CollegeEmail,
                    PhoneNumber = submitFormStuInfoVM.FormStuInfo.PhoneNumber,
                    AltPhoneNumber = submitFormStuInfoVM.FormStuInfo.AltPhoneNumber,
                    Address = new Address ()
                    {
                        Street = submitFormStuInfoVM.FormStuInfo.Address.Street,
                        City = submitFormStuInfoVM.FormStuInfo.Address.City,
                        Prov = submitFormStuInfoVM.FormStuInfo.Address.Prov,
                        Country = submitFormStuInfoVM.FormStuInfo.Address.Country,
                        PostalCode = submitFormStuInfoVM.FormStuInfo.Address.PostalCode
                    },
                    Submitted = true
                };
                _practicumFormsRepository.Add(formStuInfo);
                return RedirectToAction("Index");
            }
            return View(submitFormStuInfoVM);
        }
    }
}
