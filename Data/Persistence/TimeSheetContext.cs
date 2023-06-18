using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Data.Persistence
{
    public class TimeSheetContext : IdentityUserContext<Employee, int>
    {
        public DbSet<TimeSheet> TimeSheets { get; set; }
        public DbSet<Scope> Scopes { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Role> Roles { get; set; }

        public TimeSheetContext(DbContextOptions<TimeSheetContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}