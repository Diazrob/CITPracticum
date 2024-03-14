using System;
using System.ComponentModel.DataAnnotations;

namespace CITPracticum.Data.Enum
{
    public enum SVCredentialsCategory
    {
        [Display(Name = "IT Diploma")]
        ITDiploma,

        [Display(Name = "IS Degree")]
        ISDegree,

        [Display(Name = "CS Degree")]
        CSDegree,

        Other
    }
  
}
