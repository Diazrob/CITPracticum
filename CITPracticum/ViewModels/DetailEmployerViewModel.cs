using CITPracticum.Models;
using System.ComponentModel.DataAnnotations;

namespace CITPracticum.ViewModels
{
    public class DetailEmployerViewModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public string? SVPosition { get; set; }
        public string? OrgType { get; set; }
        public string? EmpEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Credentials { get; set; }
        public string? CredOther { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public bool? Affiliation { get; set; } = false;
        public string? EmployerComments { get; set; }
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
        public AppUser User { get; set; }
    }
}
