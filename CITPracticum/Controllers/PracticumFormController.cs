
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
            ViewData["ActivePage"] = "PracForms";
            return View();
            // test branch
        }
        // Form A submission handler
        public IActionResult FormASubmit()
        {
            var submitFormAVM = new PracticumForms();
            
            return View(submitFormAVM);
        }
        [HttpPost]
        public async Task<IActionResult> FormASubmit(PracticumForms submitFormAVM)
        {
            if (ModelState.IsValid)
            {
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
                    SVCredentialsCategory = submitFormAVM.FormA.SVCredentialsCategory,
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
                    Submitted = submitFormAVM.FormA.Submitted
                };
                _practicumFormsRepository.Add(formA);
                return RedirectToAction("Index");
            }
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
    }
}
