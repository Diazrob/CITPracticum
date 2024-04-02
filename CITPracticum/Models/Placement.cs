using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CITPracticum.Models
{
    public class Placement
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Student")]
        public int? StudentId { get; set; }
        public Student? Student { get; set; }
        [ForeignKey("Instructor")]
        public int? InstructorId { get; set; }
        public Instructor? Instructor { get; set; }
        [ForeignKey("Employer")]
        public int? EmployerId { get; set; }
        public Employer? Employer { get; set; }
        [ForeignKey("PracticumForms")]
        public int? PracticumFormsId { get; set; }
        public PracticumForms? PracticumForms { get; set; }
        [ForeignKey("Document")]
        public int? DocumentId { get; set; }
        public Document? Document { get; set; }
        [ForeignKey("JobPosting")]
        public int? JobPostingId { get; set; }
        public JobPosting? JobPosting { get; set; }
        [ForeignKey("Timesheet")]
        public int? TimesheetId { get; set; }
        public Timesheet? Timesheet { get; set; }
    }
    
}
