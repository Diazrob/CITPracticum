using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CITPracticum.Models
{
    public class FormB
    {
        [Key]
        public int Id { get; set; }
        public string PracHost { get; set; }
        public string StuName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string OrgName { get; set; }
        public string PracSV{ get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? StuSign { get; set; }
        public DateTime StuSignDate { get; set; }
        public string? EmpSign { get; set; }
        public DateTime EmpSignDate { get; set; }
        public bool Submitted { get; set; }

    }
}
