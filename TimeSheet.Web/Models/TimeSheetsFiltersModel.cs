using Services.Dtos;

namespace TimeSheet.Web.Models
{
    public class TimeSheetsFiltersModel : TimeSheetFiltersDto
    {
        public void SetTotalPage(int count, int pageSize)
        {
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
        
        public int TotalPages { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}