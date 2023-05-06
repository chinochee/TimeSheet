using Services.Dtos;

namespace TimeSheet.Web.Models
{
    public class TimeSheetTableModel
    {
        public TimeSheetTableModel(TimeSheetTableDto tableDto, TimeSheetsFiltersModel filters, EmployeeEntryDto[] employees = null)
        {
            Entries = tableDto.Entries;
            Filters = filters;
            Filters.SetTotalPage(tableDto.Total, tableDto.PageSize);
            Employees = employees;
        }

        public TimeSheetEntryDto[] Entries { get; set; }
        public TimeSheetsFiltersModel Filters { get; set; }
        public EmployeeEntryDto[]? Employees { get; set; }
    }
}