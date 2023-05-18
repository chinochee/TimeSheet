using Services.Dtos;

namespace TimeSheet.Web.Models
{
    public class EmployeeModel
    {
        public EmployeeModel(CacheDateTimeDto? cacheDateTime, EmployeeEntryBTCDto[] employees)
        {
            CacheDateTime = cacheDateTime;
            Employees = employees;
        }

        public CacheDateTimeDto? CacheDateTime { get; set; }
        public EmployeeEntryBTCDto[] Employees { get; set; }
    }
}