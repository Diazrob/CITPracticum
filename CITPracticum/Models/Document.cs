using System.ComponentModel.DataAnnotations;

namespace CITPracticum.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string StuFirstName { get; set; }
        public string StuLastName { get; set; }
        public string CollegeEmail { get; set; }
        public string StuId { get; set; }


    }
}
