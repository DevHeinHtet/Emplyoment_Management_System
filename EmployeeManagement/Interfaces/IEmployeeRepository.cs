using EmployeeManagement.Data.Entities;
using EmployeeManagement.Models.Filters;

namespace EmployeeManagement.Interfaces;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    Task<Employee> GetLatestEmployee();
    Task<IEnumerable<Employee>> GetEmployeesWithIncludeAsync(EmployeeFilter filter);
    Task<Employee> GetEmployeeByEmployeeIDAsync(string employeeID);
}
