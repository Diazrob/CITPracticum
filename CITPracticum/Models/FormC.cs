﻿using System.ComponentModel.DataAnnotations;

namespace CITPracticum.Models
{
    public class FormC
    {
        [Key]
        public int Id { get; set; }
        public string? PracSV { get; set; }
        public string? Org { get; set; }
        public string? StuName { get; set; }
        public string? A1 { get; set; }
        public string? A2 { get; set; } 
        public string? A3 { get; set; }
        public string? A4 { get; set; }
        public string? A5 { get; set; }
        public string? AComments { get; set; }
        public string? B1 { get; set; }
        public string? B2 { get; set; }
        public string? B3 { get; set; }
        public string? B4 { get; set; }
        public string? B5 { get; set; }
        public string? B6 { get; set; }
        public string? B7 { get; set; }
        public string? B8 { get; set; }
        public string? BComments { get; set; }
        public string? C1 { get; set; }
        public string? C2 { get; set; }
        public string? C3 { get; set; }
        public string? C4 { get; set; }
        public string? C5 { get; set; }
        public string? C6 { get; set; }
        public string? C7 { get; set; }
        public string? C8 { get; set; }
        public string? C9 { get; set; }
        public string? C10 { get; set; }
        public string? C11 { get; set; }
        public string? C12 { get; set; }
        public string? CComments { get; set; }
        public string? PracSVComments { get; set; }
        public string? SVSign { get; set; }
        public bool SVSubmitted { get; set; }
        public string? StuComments { get; set; }
        public string? StuSign { get; set; }
        public bool StuSubmitted { get; set; }
        public string? InsComments { get; set; }
        public string? InsSign { get; set; }
        public bool InsSubmitted { get; set; }

    }
}
