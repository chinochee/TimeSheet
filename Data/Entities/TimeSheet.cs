namespace Data.Entities
{
    public class TimeSheet
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Scope { get; set; }
        public double? WorkHours { get; set; }
        public DateTime DateOfWorks { get; set; }
        public string? Comment { get; set; }
        public DateTime DateLastEdit { get; set; }
    }
}
