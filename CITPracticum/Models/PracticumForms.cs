using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CITPracticum.Models
{
    public class PracticumForms
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("FormA")]
        public int FormAId { get; set; }
        public FormA FormA { get; set; }

    }
}
