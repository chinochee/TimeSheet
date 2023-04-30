namespace Services.Dtos
{
    public class TimeSheetFiltersDto
    {
        public string? Scope { get; set; }
        public string? NameEmployee { get; set; }
        public DateTime? DateOfWorksFrom { get; set; }
        public DateTime? DateOfWorksTo { get; set; }
        public int PageNumber { get; set; } = 1;
    }
}