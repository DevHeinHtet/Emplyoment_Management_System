using EmployeeManagement.Data.Entities;
using EmployeeManagement.DataSets;
using EmployeeManagement.Interfaces;
using EmployeeManagement.Models.Filters;

namespace EmployeeManagement.Services.Employees;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> GenerateEmployeeID()
    {
        var latestEmployee = await _unitOfWork.Employees.GetLatestEmployee();

        if (latestEmployee == null) return "E001";

        var latestEmployeeID = latestEmployee.EmployeeID;
        // Remove the 'E' prefix
        var numberPart = latestEmployeeID.Substring(1);
        var newNumber = int.Parse(numberPart) + 1;

        // Pad the new number with leading zeros to maintain 3 digits
        return "E" + newNumber.ToString("D3");
    }

    public async Task<IEnumerable<Employee>> GetEmployeesWithIncludeAsync(EmployeeFilter filter)
    {
        return await _unitOfWork.Employees.GetEmployeesWithIncludeAsync(filter);
    }

    public async Task<Employee> GetEmployeeByEmployeeIDAsync(string employeeID)
    {
        return await _unitOfWork.Employees.GetEmployeeByEmployeeIDAsync(employeeID);
    }

    public async Task AddEmployeeAsync(Employee employee)
    {
        await _unitOfWork.Employees.AddAsync(employee);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        var existingEmployee = await GetEmployeeByEmployeeIDAsync(employee.EmployeeID);
        if (employee != null)
        {
            _unitOfWork.Employees.Update(employee);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task DeleteEmployeeAsync(string employeeID)
    {
        var employee = await GetEmployeeByEmployeeIDAsync(employeeID);
        if (employee != null)
        {
            employee.IsRecordDeleted = true;
            _unitOfWork.Employees.Update(employee);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}