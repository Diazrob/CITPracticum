using System.ComponentModel.DataAnnotations;

namespace CITPracticum.ViewModels
{
    public class EditEmployerViewModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public string? EmpEmail { get; set; }
        public bool? Affiliation { get; set; }
        public string? EmployerComments { get; set;}
    }
}
