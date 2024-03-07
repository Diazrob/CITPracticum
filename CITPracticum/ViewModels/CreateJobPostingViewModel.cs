using CITPracticum.Data.Enum;

namespace CITPracticum.ViewModels
{
    public class CreateJobPostingViewModel
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string DueDate { get; set; }
        public string Company { get; set; }
        public PaymentCategory PaymentCategory { get; set; }
        public string JobLink { get; set; }
        public string Location { get; set; }
    }
}
