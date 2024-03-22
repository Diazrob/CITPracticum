using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CITPracticum.Models
{
    public class AppUser : IdentityUser
    {
        [Key]
        public int Id { get; set; }
        public string? ProfileImage { get; set; }

        [ForeignKey("Administrator")]
        public int? AdministratorId { get; set; }
        public Administrator? Administrator { get; set; }

        [ForeignKey("Student")]
        public int? StudentId { get; set; }
        public Student? Student { get; set; }

        [ForeignKey("Employer")]
        public int? EmployerId { get; set; }
        public Employer? Employer { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Employer> Employers { get; set; }
    }
}
