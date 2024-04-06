using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CITPracticum.Models
{
    public class Timesheet
    {
        [Key]
        public int Id { get; set; }

        //[ForeignKey("TimeEntry")]
        //public int? TimeEntryID { get; set; }
        //public TimeEntry TimeEntry { get; set; }
        public List<TimeEntry>? TimeEntries { get; set; }
        public decimal TotalHours => TimeEntries?.Sum(entry => entry.Hours) ?? 0;
    }
}
