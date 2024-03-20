using System.ComponentModel.DataAnnotations;

namespace CITPracticum.Models
{
    public class Administrator
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AdminEmail { get; set; }
    }
}
