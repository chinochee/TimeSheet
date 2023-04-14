using Services.Configuration;

namespace Services.Dtos
{
    public class TimeSheetTableDto
    {
        public TimeSheetEntryDto[] Entries { get; set; }
        public int Total { get; set; }
        public int PageSize = TableSettings.PageSize;
    }
}
