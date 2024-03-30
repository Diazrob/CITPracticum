using System.ComponentModel.DataAnnotations;

namespace CITPracticum.Models
{
    public class Timesheet
    {
        [Key]
        public int Id { get; set; }
        public List<TimeEntry>? TimeEntries { get; set; }
    }
}
