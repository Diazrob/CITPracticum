using CITPracticum.Data.Enum;
using CITPracticum.Models;
using System.ComponentModel.DataAnnotations;

namespace CITPracticum.ViewModels
{
    public class CreateFormAViewModel
    {
        public string StuLastName { get; set; }
        public string StuFirstName { get; set; }
        public string StuId { get; set; }
        public string Program { get; set; }
        public string HostCompany { get; set; }
        public string OrgType { get; set; }
        public string SVName { get; set; }
        public string SVPosition { get; set; }
        [DataType(DataType.EmailAddress)]
        public string SVEmail { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string SVPhoneNumber { get; set; }
        public string? SVCredentials { get; set; }
        public string? SVCredOther { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public DateTime StartDate { get; set; }
        public PaymentCategory PaymentCategory { get; set; }
        public string OutOfCountry { get; set; }
        public bool Submitted { get; set; }
    }
}
