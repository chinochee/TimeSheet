using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class TimeSheet
    {
        public int Id { get; set; }
        public string? NameEmployee { get; set; }
        [ForeignKey("Scope")]
        public int ScopeId { get; set; }
        public Scope Scope { get; set; }
        public double? WorkHours { get; set; }
        public DateTime DateOfWorks { get; set; }
        public string? Comment { get; set; }
        public DateTime DateLastEdit { get; set; }
    }
}
