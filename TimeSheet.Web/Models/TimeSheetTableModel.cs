using Services.Dtos;

namespace TimeSheet.Web.Models
{
    public class TimeSheetTableModel
    {
        public TimeSheetTableModel(TimeSheetTableDto tableDto, TimeSheetsFiltersModel filters)
        {
            Entries = tableDto.Entries;
            Filters = filters;
            Filters.SetTotalPage(tableDto.Total, tableDto.PageSize);
        }

        public TimeSheetEntryDto[] Entries { get; set; }
        public TimeSheetsFiltersModel Filters { get; set; }
    }
}
