using EmployeeManagement.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.EmployeeID);

        builder.Property(e => e.EmployeeID)
            .HasMaxLength(5)
            .HasColumnType("VARCHAR")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.EmployeeName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("VARCHAR(100)");

        builder.Property(e => e.BirthDate)
            .IsRequired();

        builder.Property(e => e.Gender)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnType("VARCHAR(10)");

        builder.HasOne(e => e.Position)
            .WithMany()
            .HasForeignKey(e => e.PositionID);

        builder.Property(e => e.RegistrationDate)
            .IsRequired();

        builder.Property(e => e.RecommendationLetter)
            .HasColumnType("NVARCHAR(MAX)");

        builder.Property(e => e.EmployeePhoto)
            .HasColumnType("NVARCHAR(MAX)");

        builder.Property(e => e.NrcFrontImage)
            .HasColumnType("NVARCHAR(MAX)");

        builder.Property(e => e.NrcBackImage)
            .HasColumnType("NVARCHAR(MAX)");

        builder.Property(e => e.IsRecordDeleted)
            .HasDefaultValue(false);
    }
}
