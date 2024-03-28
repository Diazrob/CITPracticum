using CITPracticum.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CITPracticum.ViewModels
{
    public class CreateFormStuInfoViewModel
    {
        [Required(ErrorMessage = "Student Last Name is required")]
        public string StuLastName { get; set; }
        [Required(ErrorMessage = "Student First Name is required")]
        public string StuFirstName { get; set; }
        [Required(ErrorMessage = "Student Id is required")]
        public string StuId { get; set; }
        [Required(ErrorMessage = "Program is required")]
        public string Program { get; set; }
        [Required(ErrorMessage = "Program start date is required")]
        public string ProgStartDate { get; set; }
        [Required(ErrorMessage = "Practicum start date is required")]
        public string PracStartDate { get; set; }
        [Required(ErrorMessage = "Student email is required")]
        [DataType(DataType.EmailAddress)]
        public string CollegeEmail { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Alternate phone number is required")]
        [DataType(DataType.PhoneNumber)]
        public string AltPhoneNumber { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public CreateAddressViewModel CreateAddressViewModel { get; set; }
        public bool Submitted { get; set; }
    }
}
