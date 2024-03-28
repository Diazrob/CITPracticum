using System.ComponentModel.DataAnnotations;

namespace CITPracticum.Models
{
    public class FormFOIP
    {
        [Key]
        public int Id { get; set; }
        public string StuFirstName { get; set; }
        public string StuLastName { get; set; }
        public string StuId { get; set; }
        public string Program { get; set; }
        public string? Other { get; set; }
        public string StuSign { get; set; }
        public DateTime StuSignDate { get; set; }
        public bool Acknowledged { get; set; }
        public bool Submitted { get; set; }

    }
}
