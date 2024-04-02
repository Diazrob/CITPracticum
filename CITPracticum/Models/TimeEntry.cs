using System.ComponentModel.DataAnnotations;

namespace CITPracticum.Models
{
    public class TimeEntry
    {
        [Key]
        public int Id { get; set; }
        public DateTime ShiftDate { get; set; }
        public string? Description { get; set; }
        public decimal Hours { get; set; }
        public decimal? HoursToDate { get; set; }
    }
}
