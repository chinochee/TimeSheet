using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasOne(c => c.Employee)
                .WithMany()
                .HasForeignKey(k => k.EmployeeId)
                .IsRequired();
        }
    }
}