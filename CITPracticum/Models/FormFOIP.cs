﻿using System.ComponentModel.DataAnnotations;

namespace CITPracticum.Models
{
    public class FormFOIP
    {
        [Key]
        public int Id { get; set; }
        public string StuFirstName { get; set; }
        public string StuLastName { get; set; }
        public string StuId { get; set; }
        public string Program { get; set; }
        public string? Other { get; set; }
        public bool StuSign { get; set; }
        public DateOnly StuSignDate { get; set; }
        public bool Submitted { get; set; }

    }
}