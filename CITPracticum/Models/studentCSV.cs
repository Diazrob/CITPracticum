using CsvHelper.Configuration.Attributes;

namespace CITPracticum.Models
{
    public class studentCSV
    {
        [Index(0)]
        public string StuName { get; set; }
        [Index(1)]
        public string StuId { get; set; }
        [Index(2)]
        public string StuSISId  { get; set; }
        [Index(3)]
        public string Email { get; set; }
        [Index(4)]
        public string Section { get; set; }

    }
}
