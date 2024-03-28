using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CITPracticum.Models
{
    public class FormStuInfo
    {
        [Key]
        public int Id { get; set; }
        public string? StuLastName { get; set; }
        public string? StuFirstName { get; set; }
        public string? StuId { get; set; }
        public string? Program { get; set; }
        public string? ProgStartDate { get; set; }
        public string? PracStartDate { get; set; }
        public string? CollegeEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AltPhoneNumber { get; set; }
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public bool Submitted { get; set; }

    }
}
