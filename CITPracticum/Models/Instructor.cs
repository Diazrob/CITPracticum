using System.ComponentModel.DataAnnotations;

namespace CITPracticum.Models
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? InstructorEmail { get; set; }
    }
}
