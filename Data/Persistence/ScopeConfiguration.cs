using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence
{
    public class ScopeConfiguration : IEntityTypeConfiguration<Scope>
    {
        public void Configure(EntityTypeBuilder<Scope> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasOne(c => c.Currency)
                .WithMany()
                .HasForeignKey()
                .IsRequired();
        }
    }
}
