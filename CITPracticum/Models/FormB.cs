using System.ComponentModel.DataAnnotations.Schema;

namespace CITPracticum.Models
{
    public class FormB
    {
        public int Id { get; set; }
        public string PracHost { get; set; }
        public string StuName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string OrgName { get; set; }
        public string PracSV{ get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool StuSign { get; set; }
        public DateOnly StuSignDate { get; set; }
        public bool EmpSign { get; set; }
        public DateOnly EmpSignDate { get; set; }
        public bool Submitted { get; set; }

    }
}
