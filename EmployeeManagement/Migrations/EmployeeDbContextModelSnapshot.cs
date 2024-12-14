﻿// <auto-generated />
using System;
using EmployeeManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EmployeeManagement.Migrations
{
    [DbContext(typeof(EmployeeDbContext))]
    partial class EmployeeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.36")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EmployeeManagement.Data.Entities.Department", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentID"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)");

                    b.HasKey("DepartmentID");

                    b.ToTable("Departments");

                    b.HasData(
                        new
                        {
                            DepartmentID = 1,
                            Name = "Admin"
                        },
                        new
                        {
                            DepartmentID = 2,
                            Name = "HR"
                        },
                        new
                        {
                            DepartmentID = 3,
                            Name = "Design"
                        },
                        new
                        {
                            DepartmentID = 4,
                            Name = "Information Technology"
                        },
                        new
                        {
                            DepartmentID = 5,
                            Name = "Marketing"
                        });
                });

            modelBuilder.Entity("EmployeeManagement.Data.Entities.Employee", b =>
                {
                    b.Property<string>("EmployeeID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(5)
                        .HasColumnType("VARCHAR(5)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("EmployeePhoto")
                        .HasColumnType("NVARCHAR(MAX)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("VARCHAR(10)");

                    b.Property<bool>("IsRecordDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("NrcBackImage")
                        .HasColumnType("NVARCHAR(MAX)");

                    b.Property<string>("NrcFrontImage")
                        .HasColumnType("NVARCHAR(MAX)");

                    b.Property<int>("PositionID")
                        .HasColumnType("int");

                    b.Property<string>("RecommendationLetter")
                        .HasColumnType("NVARCHAR(MAX)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("EmployeeID");

                    b.HasIndex("PositionID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("EmployeeManagement.Data.Entities.Position", b =>
                {
                    b.Property<int>("PositionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PositionID"), 1L, 1);

                    b.Property<int>("DepartmentID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)");

                    b.HasKey("PositionID");

                    b.HasIndex("DepartmentID");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("EmployeeManagement.Data.Entities.Employee", b =>
                {
                    b.HasOne("EmployeeManagement.Data.Entities.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Position");
                });

            modelBuilder.Entity("EmployeeManagement.Data.Entities.Position", b =>
                {
                    b.HasOne("EmployeeManagement.Data.Entities.Department", "Department")
                        .WithMany("Positions")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("EmployeeManagement.Data.Entities.Department", b =>
                {
                    b.Navigation("Positions");
                });
#pragma warning restore 612, 618
        }
    }
}
