using Microsoft.AspNetCore.Identity;

namespace CITPracticum.Models
{
    public class AppUser : IdentityUser
    {
        public string? ProfileImage { get; set; }
        public string? CompanyName { get; set; }
        public string? StudentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Employer> Employers { get; set; }
    }
}
