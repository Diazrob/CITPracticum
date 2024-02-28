using System.ComponentModel.DataAnnotations;

namespace CITPracticum.Models
{
    public class Timesheet
    {
        [Key]
        public int Id { get; set; }
        public DateTime ShiftDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Hours { get; set; }

    }
}
