
using CITPracticum.Data.Enum;
using CITPracticum.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CITPracticum.Models
{
    public class FormA
    {
        [Key]
        public int? Id { get; set; }
        public string StuLastName { get; set; }
        public string StuFirstName { get; set; }
        public string StuId { get; set; }
        public string Program { get; set; }
        public string HostCompany { get; set; }
        public string OrgType { get; set; }
        public string SVName { get; set; }
        public string SVPosition { get; set; }
        public string SVEmail { get; set; }
        public string SVPhoneNumber { get; set; }
        public string? SVCredentials { get; set; }
        public string? SVCredOther { get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public DateTime StartDate { get; set; }
        public PaymentCategory PaymentCategory { get; set; }
        public bool Submitted { get; set; }

    }
}
