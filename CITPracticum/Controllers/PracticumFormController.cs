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
            // test branch
        }

        public IActionResult FormASubmit()
        {
            var submitFormAVM = new SubmitFormAViewModel();
            return View(submitFormAVM);
        }
        [HttpPost]
        public async Task<IActionResult> FormASubmit(SubmitFormAViewModel submitFormAVM)
        {
            if (ModelState.IsValid)
            {
                var formA = new FormA()
                {
                    StuLastName = submitFormAVM.StuLastName,
                    StuFirstName = submitFormAVM.StuFirstName,
                    StuId = submitFormAVM.StuId,
                    Program = "Computer Information Technology",
                    HostCompany = submitFormAVM.HostCompany,
                    OrgType = submitFormAVM.OrgType,
                    SVName = submitFormAVM.SVName,
                    SVPosition = submitFormAVM.SVPosition,
                    SVEmail = submitFormAVM.SVEmail,
                    SVPhoneNumber = submitFormAVM.SVPhoneNumber,
                    SVCredentials = submitFormAVM.SVCredentials,
                    SVCredOther = submitFormAVM.SVCredOther,
                    Address = new Address()
                    {
                        Street = submitFormAVM.Address.Street,
                        City = submitFormAVM.Address.City,
                        Prov = submitFormAVM.Address.Prov,
                        Country = submitFormAVM.Address.Country,
                        PostalCode = submitFormAVM.Address.PostalCode
                    },
                    StartDate = submitFormAVM.StartDate,
                    PaymentCategory = submitFormAVM.PaymentCategory,
                    Submitted = submitFormAVM.Submitted
                };
                _practicumFormsRepository.Add(formA);
                return RedirectToAction("Index");
            }
            return View(submitFormAVM);
        }
        public IActionResult EmployerSubmittedForms()
        {
            return View();
        }
        public IActionResult StudentSubmittedForms()
        {
            return View();
        }
    }
}
