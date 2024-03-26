using CITPracticum.Data.Enum;
using CITPracticum.Models;
using System.ComponentModel.DataAnnotations;

namespace CITPracticum.ViewModels
{
    public class CreateFormAViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StuLastName { get; set; }
        [Required]
        public string StuFirstName { get; set; }
        [Required]
        public string StuId { get; set; }
        [Required]
        public string Program { get; set; }
        [Required]
        public string HostCompany { get; set; }
        [Required]
        public string OrgType { get; set; }
        [Required]
        public string SVName { get; set; }
        [Required]
        public string SVPosition { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string SVEmail { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string SVPhoneNumber { get; set; }
        [Required]
        public string? SVCredentials { get; set; }
        [Required]
        public string? SVCredOther { get; set; }
        [Required]
        public int AddressId { get; set; }
        [Required]
        public Address Address { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public PaymentCategory PaymentCategory { get; set; }
        [Required]
        public string OutOfCountry { get; set; }
        [Required]
        public bool Submitted { get; set; }
    }
}
