using System.ComponentModel.DataAnnotations;

namespace CITPracticum.ViewModels
{
    public class CreateFormDViewModel
    {
        public string PracSV { get; set; }
        public string Org { get; set; }
        public string StuName { get; set; }
        [Required]
        public string A1 { get; set; }
        [Required]
        public string A2 { get; set; }
        [Required]
        public string A3 { get; set; }
        [Required]
        public string A4 { get; set; }
        [Required]
        public string A5 { get; set; }
        [Required]
        public string AComments { get; set; }
        [Required]
        public string B1 { get; set; }
        [Required]
        public string B2 { get; set; }
        [Required]
        public string B3 { get; set; }
        [Required]
        public string B4 { get; set; }
        [Required]
        public string B5 { get; set; }
        [Required]
        public string B6 { get; set; }
        [Required]
        public string B7 { get; set; }
        [Required]
        public string B8 { get; set; }
        [Required]
        public string BComments { get; set; }
        [Required]
        public string C1 { get; set; }
        [Required]
        public string C2 { get; set; }
        [Required]
        public string C3 { get; set; }
        [Required]
        public string C4 { get; set; }
        [Required]
        public string C5 { get; set; }
        [Required]
        public string C6 { get; set; }
        [Required]
        public string C7 { get; set; }
        [Required]
        public string C8 { get; set; }
        [Required]
        public string C9 { get; set; }
        [Required]
        public string C10 { get; set; }
        [Required]
        public string C11 { get; set; }
        [Required]
        public string C12 { get; set; }
        [Required]
        public string CComments { get; set; }
        [Required]
        public string? PracSVComments { get; set; }
        [Required]
        public string? SVSign { get; set; }
        public bool SVSubmitted { get; set; }
        [Required]
        public string? StuComments { get; set; }
        [Required]
        public string? StuSign { get; set; }
        public bool StuSubmitted { get; set; }
        [Required]
        public string? InsComments { get; set; }
        [Required]
        public string? InsSign { get; set; }
        public bool InsSubmitted { get; set; }
    }
}
