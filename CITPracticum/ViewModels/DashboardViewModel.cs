using CITPracticum.Models;

namespace CITPracticum.ViewModels
{
    public class DashboardViewModel
    {
        public int? StudentId { get; set; }
        public Student? Student { get; set; }
        public int? PracticumFormsId { get; set; }
        public PracticumForms? PracticumForms { get; set; }
        public int? DocumentId { get; set; }
        public Document? Document { get; set; }
        public int? TimesheetId { get; set; }
        public Timesheet? Timesheet { get; set; }
        public int PlacementCount { get; set; }
        public int FormFOIPCount { get; set; }
        public string FOIPPercentage { get; set; }
        public int FormIdCount { get; set; }
        public string IdPercentage { get; set; }
        public int FormACount { get; set; }
        public string APercentage { get; set; }
        public int FormBCount { get; set; }
        public string BPercentage { get; set; }
        public int FormCCount { get; set; }
        public string CPercentage { get; set; }
        public int FormDCount { get; set; }
        public string DPercentage { get; set; }
        public int ResumeCount { get; set; }
        public string ResumePercentage { get; set; }
        public int CoverLetterCount { get; set; }
        public string CLPercentage { get; set; }
        public int OneHundredHoursCount { get; set; }
        public string HunHrsPercentage { get; set; }
        public int TwoHundredHoursCount { get; set; }
        public string TwohunHrsPercentage { get; set; }
        // int percentage
        public int FOIPPercent { get; set; }
        public int IdPercent { get; set; }
        public int APercent { get; set; }
        public int BPercent { get; set; }
        public int CPercent { get; set; }
        public int DPercent { get; set; }
    }
}
