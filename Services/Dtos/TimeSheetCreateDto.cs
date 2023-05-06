namespace Services.Dtos
{
    public class TimeSheetCreateDto
    {
        public int EmployeeId { get; set; }
        public int ScopeId { get; set; }
        public double? WorkHours { get; set; }
        public DateTime DateOfWorks { get; set; }
        public string? Comment { get; set; }
    }
}