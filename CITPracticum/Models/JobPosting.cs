using CITPracticum.Data.Enum;
using Microsoft.AspNetCore.Builder;
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
        public bool Archived { get; set; }
        public Employer? Employer { get; set; }
        public int? EmployerId { get; set; }
        public ICollection<Application> JobApplications { get; set; } = new List<Application>();
    }
}
