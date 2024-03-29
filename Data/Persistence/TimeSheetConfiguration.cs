﻿using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence
{
    public class TimeSheetConfiguration : IEntityTypeConfiguration<TimeSheet>
    {
        public void Configure(EntityTypeBuilder<TimeSheet> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.WorkHours).HasPrecision(4, 2);
            builder.HasOne(c => c.Scope)
                .WithMany(c => c.TimeSheetList)
                .HasForeignKey(k => k.ScopeId)
                .IsRequired();
            builder.HasOne(c => c.Employee)
                .WithMany(c => c.TimeSheetList)
                .HasForeignKey(k => k.EmployeeId)
                .IsRequired();
        }
    }
}