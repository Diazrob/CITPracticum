using CITPracticum.Models;
using System.ComponentModel.DataAnnotations;

namespace CITPracticum.ViewModels
{
    public class CreateFormBViewModel
    {
        [Required(ErrorMessage = "PracticumForms host is required")]
        public string PracHost { get; set; }
        [Required(ErrorMessage = "Student Name is required")]
        public string StuName { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Organization name is required")]
        public string OrgName { get; set; }
        [Required(ErrorMessage = "Supervisor name is required")]
        public string PracSV { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public Address Address { get; set; }
        [Required(ErrorMessage = "Supervisor position is required")]
        public string Position { get; set; }
        [Required(ErrorMessage = "Supervisor email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Supervisor phone number is required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Student signature is required")]
        public bool StuSign { get; set; }
        public DateTime StuSignDate { get; set; }
        [Required(ErrorMessage = "Employee signature is required")]
        public bool EmpSign { get; set; }
        public DateTime EmpSignDate { get; set; }
    }
}
