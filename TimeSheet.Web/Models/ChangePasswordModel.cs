using Services.Dtos;

namespace TimeSheet.Web.Models
{
    public class ChangePasswordModel
    {
        public ChangePasswordModel(EmployeeEntryDto[] employees = null)
        {
            Employees = employees;
        }
        public ChangePasswordModel() { }
        
        public LoginEditDto Login { get; set; }
        public EmployeeEntryDto[]? Employees { get; set; }
    }
}