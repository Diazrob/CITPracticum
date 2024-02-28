using Microsoft.AspNetCore.Identity;

namespace CITPracticum.Models
{
    public class AppUser : IdentityUser
    {
        public string? ProfileImageUrl { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Employer> Employers { get; set; }
    }
}
