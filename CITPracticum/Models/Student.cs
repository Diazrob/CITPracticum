using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CITPracticum.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? StuId { get; set; }
        public string? StuEmail { get; set; }
        [ForeignKey("Document")]
        public int? DocumentId { get; set; }
        public Document? Document { get; set; }

    }
}
