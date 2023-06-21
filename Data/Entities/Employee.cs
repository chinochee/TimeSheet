using Microsoft.AspNetCore.Identity;

namespace Data.Entities
{
    public class Employee : IdentityUser<int>
    {
        public string Name { get; set; }
        public ICollection<TimeSheet> TimeSheetList { get; set; }
        public ICollection<Role> RoleList { get; set; }
    }
}