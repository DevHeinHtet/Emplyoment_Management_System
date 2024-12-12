using EmployeeManagement.Data.Entities;
using EmployeeManagement.Interfaces;

namespace EmployeeManagement.Services.Departments;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Department>> GetDepartmentsAsync()
    {
        return await _unitOfWork.Departments.GetAllAsync();
    }
}