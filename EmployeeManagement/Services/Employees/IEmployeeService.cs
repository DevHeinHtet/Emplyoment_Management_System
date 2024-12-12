using EmployeeManagement.Data.Entities;
using EmployeeManagement.Models.Filters;

namespace EmployeeManagement.Services.Employees;

public interface IEmployeeService
{
    Task<string> GenerateEmployeeID();
    Task<IEnumerable<Employee>> GetEmployeesWithIncludeAsync(EmployeeFilter filter);
    Task<Employee> GetEmployeeByEmployeeIDAsync(string employeeID);
    Task AddEmployeeAsync(Employee employee);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(string employeeID);
}