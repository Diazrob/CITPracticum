using System.ComponentModel.DataAnnotations;

namespace CITPracticum.ViewModels
{
    public class RegisterEmployerViewModel
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Supervisor's position")]
        [Required(ErrorMessage = "Supervisor's position is required")]
        public string SVPosition { get; set; }
        [Display(Name = "Type of Organization")]
        [Required(ErrorMessage = "Type of Organization is required")]
        public string OrgType { get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Display(Name = "Supervisor Credentials")]
        [Required(ErrorMessage = "Supervisor credentials is required")]
        public string Credentials { get; set; }
        public string? CredOther { get; set; }
        public CreateAddressViewModel CreateAddressViewModel { get; set; }
        public bool Affiliation { get; set; }

    }
}
