
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class PracticumFormController : Controller
    {
        private readonly IPracticumFormsRepository _practicumFormsRepository;

        public PracticumFormController(IPracticumFormsRepository practicumFormsRepository)
        {
            _practicumFormsRepository = practicumFormsRepository;
        }
        public IActionResult Index()
        {   
            return View();
        }

        public IActionResult EmployerSubmittedForms()
        {
            return View();
        }
        public IActionResult StudentSubmittedForms()
        {
            return View();
        }

        // Form A submission handler
        public IActionResult FormASubmit()
        {
            var submitFormAVM = new PracticumForms();
            
            return View(submitFormAVM);
        }
        [HttpPost]
        public async Task<IActionResult> FormASubmit(PracticumForms submitFormAVM, List<string> credentialsList)
        {
            if (ModelState.IsValid)
            {
                string credentials = string.Join(", ", credentialsList);

                var formA = new FormA()
                {
                    StuLastName = submitFormAVM.FormA.StuLastName,
                    StuFirstName = submitFormAVM.FormA.StuFirstName,
                    StuId = submitFormAVM.FormA.StuId,
                    Program = "Computer Information Technology",
                    HostCompany = submitFormAVM.FormA.HostCompany,
                    OrgType = submitFormAVM.FormA.OrgType,
                    SVName = submitFormAVM.FormA.SVName,
                    SVPosition = submitFormAVM.FormA.SVPosition,
                    SVEmail = submitFormAVM.FormA.SVEmail,
                    SVPhoneNumber = submitFormAVM.FormA.SVPhoneNumber,
                    SVCredentials = credentials,
                    SVCredOther = submitFormAVM.FormA.SVCredOther,
                    Address = new Address()
                    {
                        Street = submitFormAVM.FormA.Address.Street,
                        City = submitFormAVM.FormA.Address.City,
                        Prov = submitFormAVM.FormA.Address.Prov,
                        Country = submitFormAVM.FormA.Address.Country,
                        PostalCode = submitFormAVM.FormA.Address.PostalCode
                    },
                    StartDate = submitFormAVM.FormA.StartDate,
                    PaymentCategory = submitFormAVM.FormA.PaymentCategory,
                    Submitted = true
                };
                _practicumFormsRepository.Add(formA);
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
        public async Task<IActionResult> FormCSubmit(PracticumForms submitFormCVM, List<string> credentialsList)
        {
            if (ModelState.IsValid)
            { 
                var formC = new FormC()
                {
                    PracSV = submitFormCVM.FormC.PracSV,
                    Org = submitFormCVM.FormC.Org,
                    StuName = submitFormCVM.FormC.StuName,


                };
                _practicumFormsRepository.Add(formC);
                return RedirectToAction("Index");
            }
            return View(submitFormCVM);
        }
    }
}
