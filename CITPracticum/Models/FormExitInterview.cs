using System.ComponentModel.DataAnnotations;

namespace CITPracticum.Models
{
    public class FormExitInterview
    {
        [Key]
        public int Id { get; set; }
        public string StuName { get; set; }
        public DateTime SignDate { get; set; }
        public string InsName { get; set; }
        public string A1 { get; set; }
        public string A2 { get; set; }
        public string A3 { get; set; }
        public string A4 { get; set; }
        public string A5 { get; set; }
        public string A6 { get; set; }

    }
}
