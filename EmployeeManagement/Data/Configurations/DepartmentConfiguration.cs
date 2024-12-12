using EmployeeManagement.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Data.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(d => d.DepartmentID);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("VARCHAR(100)");
    }
}