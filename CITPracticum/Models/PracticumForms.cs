using CITPracticum.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CITPracticum.Models
{
    public class PracticumForms
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("FormA")]
        public int? FormAId { get; set; }
        public FormA? FormA { get; set; }

        [ForeignKey("FormB")]
        public int? FormBId { get; set; }
        public FormB? FormB { get; set; }

        [ForeignKey("FormC")]
        public int? FormCId { get; set; }
        public FormC? FormC { get; set; }

        [ForeignKey("FormD")]
        public int? FormDId { get; set; }
        public FormD? FormD { get; set; }

        [ForeignKey("FormFOIP")]
        public int? FormFOIPId { get; set; }
        public FormFOIP? FormFOIP { get; set; }

        [ForeignKey("FormStuInfo")]
        public int? FormStuInfoId { get; set; }
        public FormStuInfo? FormStuInfo { get; set; }

        // View Models
        public CreateFormAViewModel CreateFormAViewModel { get; set; }

    }
}
