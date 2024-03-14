using CITPracticum.Data.Enum;
using CITPracticum.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CITPracticum.ViewModels
{
    public class SubmitFormAViewModel
    {
        public int Id { get; set; }
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
        public string SVCredentials { get; set; }
        public string? SVCredOther { get; set; }
        public Address Address { get; set; }
        public DateOnly StartDate { get; set; }
        public PaymentCategory PaymentCategory { get; set; }
        public bool Submitted { get; set; }
    }
}
