using System.ComponentModel.DataAnnotations;

namespace CITPracticum.ViewModels
{
    public class CreateFormFOIPViewModel
    {
        [Required(ErrorMessage = "Student First Name is required")]
        public string StuFirstName { get; set; }
        [Required(ErrorMessage = "Student Last Name is required")]
        public string StuLastName { get; set; }
        [Required(ErrorMessage = "Student Id is required")]
        public string StuId { get; set; }
        [Required(ErrorMessage = "Program is required")]
        public string Program { get; set; }
        public string? Other { get; set; }
        [Required(ErrorMessage = "Student signature is required")]
        public string StuSign { get; set; }
        [Required(ErrorMessage = "Date signed is required")]
        public DateTime StuSignDate { get; set; }
        [Required]
        public bool Acknowledged { get; set; }
    }
}
