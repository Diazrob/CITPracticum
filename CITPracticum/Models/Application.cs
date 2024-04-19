using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CITPracticum.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }
        [ForeignKey("JobPosting")]
        public int JobPostingId { get; set; }
        public JobPosting JobPosting { get; set; }
    }
}