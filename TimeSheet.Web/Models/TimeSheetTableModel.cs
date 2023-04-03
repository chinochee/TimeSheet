using Services.Dtos;

namespace TimeSheet.Web.Models
{
    public class TimeSheetTableModel
    {
        public TimeSheetEntryDto[] Entries { get; set; }
        public TimeSheetFiltersDto Filters { get; set; }
    }
}
