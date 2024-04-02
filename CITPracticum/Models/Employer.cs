using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CITPracticum.Models
{
    public class Employer
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public string? SVPosition { get; set; }
        public string? OrgType  { get; set; }
        public string? EmpEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Credentials { get; set; }
        public string? CredOther { get; set; }
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public bool? Affiliation { get; set; }

    }
}
