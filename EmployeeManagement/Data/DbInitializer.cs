using EmployeeManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Data;

public static class DbInitializer
{
    public static void Departments(ModelBuilder builder)
    {
        var departments = new[]
        {
            new Department { DepartmentID = 1, Name = "Admin" },
            new Department { DepartmentID = 2, Name = "HR" },
            new Department { DepartmentID = 3, Name = "Design" },
            new Department { DepartmentID = 4, Name = "Information Technology" },
            new Department { DepartmentID = 5, Name = "Marketing" }
        };

        builder.Entity<Department>().HasData(departments);
    }
}