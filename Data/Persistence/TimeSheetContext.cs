using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Data.Persistence
{
    public class TimeSheetContext : DbContext
    {
        public DbSet<TimeSheet> TimeSheets { get; set; }
        public DbSet<Scope> Scopes { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }

        public TimeSheetContext(DbContextOptions<TimeSheetContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}