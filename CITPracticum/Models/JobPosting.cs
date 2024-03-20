using CITPracticum.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace CITPracticum.Models
{
    public class JobPosting
    {
        [Key]
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string Deadline { get; set; }
        public string Company { get; set; }
        public PaymentCategory PaymentCategory { get; set; }
        public string Location { get; set; }
        public string Link { get; set; }
    }
}
