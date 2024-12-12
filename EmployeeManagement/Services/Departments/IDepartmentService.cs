using EmployeeManagement.Data.Entities;

namespace EmployeeManagement.Services.Departments;

public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetDepartmentsAsync();
}
