﻿using Microsoft.AspNetCore.Identity;

namespace Data.Entities
{
    public class Employee : IdentityUser<int>
    {
        public string Name { get; set; }
        public ICollection<TimeSheet> TimeSheetList { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}