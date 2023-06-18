﻿using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasMany(e => e.TimeSheetList)
                .WithOne(t => t.Employee)
                .HasForeignKey(t => t.EmployeeId)
                .IsRequired();
            builder.HasOne(e => e.Role)
                .WithMany()
                .HasForeignKey(k => k.RoleId)
                .IsRequired();
        }
    }
}