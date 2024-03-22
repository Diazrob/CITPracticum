using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace CITPracticum.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string? Resume { get; set; }
        public string? CoverLetter { get; set; }
    }
}
