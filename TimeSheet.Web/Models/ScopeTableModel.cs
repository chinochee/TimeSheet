using Services.Dtos;

namespace TimeSheet.Web.Models
{
    public class ScopeTableModel
    {
        public ScopeTableModel(ScopeTableDto tableDto, ScopesFiltersModel filters)
        {
            Entries = tableDto.Entries;
            Filters = filters;
            Filters.SetTotalPage(tableDto.Total, tableDto.PageSize);
        }

        public ScopeEntryDto[] Entries { get; set; }
        public ScopesFiltersModel Filters { get; set; }
    }
}