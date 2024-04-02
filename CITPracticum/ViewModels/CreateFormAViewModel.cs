using CITPracticum.Data.Enum;
using CITPracticum.Models;
using System.ComponentModel.DataAnnotations;

namespace CITPracticum.ViewModels
{
    public class CreateFormAViewModel
    {
        [Required(ErrorMessage = "Last Name is required")]
        public string StuLastName { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string StuFirstName { get; set; }
        [Required(ErrorMessage = "Student Id is required")]
        public string StuId { get; set; }
        [Required(ErrorMessage = "Program is required")]
        public string Program { get; set; }
        [Required(ErrorMessage = "Host Company is required")]
        public string HostCompany { get; set; }
        [Required(ErrorMessage = "Organization Type is required")]
        public string OrgType { get; set; }
        [Required(ErrorMessage = "Surpervisor firstname is required is required")]
        public string SVFirstName { get; set; }
        [Required(ErrorMessage = "Surpervisor lastname is required is required")]
        public string SVLastName { get; set; }
        [Required(ErrorMessage = "Supervisor position is required")]
        public string SVPosition { get; set; }
        [Required(ErrorMessage = "Supervisor email address is required")]
        [DataType(DataType.EmailAddress)]
        public string SVEmail { get; set; }
        [Required(ErrorMessage = "Supervisor contact number is required")]
        [DataType(DataType.PhoneNumber)]
        public string SVPhoneNumber { get; set; }
        [Required(ErrorMessage = "Supervisor credential is required")]
        public string? SVCredentials { get; set; }
        public string? SVCredOther { get; set; }
        public CreateAddressViewModel CreateAddressViewModel { get; set; }
        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Payment Type is required")]
        public PaymentCategory PaymentCategory { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public YesNoCategory OutOfCountry { get; set; }
    }
}

