using CITPracticum.Data.Enum;
using CITPracticum.Models;

namespace CITPracticum.ViewModels
{
    public class CreateJobPostingViewModel
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string Deadline { get; set; }
        public string Company { get; set; }
        public PaymentCategory PaymentCategory { get; set; }
        public string JobLink { get; set; }
        public string Location { get; set; }
        public Employer? Employer { get; set; }
        public int? EmployerId { get; set; }
    }
}
