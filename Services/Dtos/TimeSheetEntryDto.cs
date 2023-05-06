namespace Services.Dtos
{
    public class TimeSheetEntryDto
    {
        public int Id { get; set; }
        public string? NameEmployee { get; set; }
        public string? NameScope { get; set; }
        public double? WorkHours { get; set; }
        public string? DateOfWorks { get; set; }
        public string? Comment { get; set; }
    }
}