namespace Services.Dtos
{
    public class TimeSheetFiltersDto
    {
        public string? Scope { get; set; }
        public DateTime? DateOfWorksFrom { get; set; }
        public DateTime? DateOfWorksTo { get; set; }
    }
}
