using EmployeeManagement.Data;
using EmployeeManagement.Interfaces;

namespace EmployeeManagement.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly EmployeeDbContext _context;
    private IPositionRepository _positions { get; }
    private IEmployeeRepository _employees { get; }
    private IDepartmentRepository _departments { get; }

    public IPositionRepository Positions => _positions;
    public IEmployeeRepository Employees => _employees;
    public IDepartmentRepository Departments => _departments;

    public UnitOfWork(
        EmployeeDbContext context,
        IPositionRepository positions,
        IEmployeeRepository employees,
        IDepartmentRepository departments)
    {
        _context = context;
        _positions = positions;
        _employees = employees;
        _departments = departments;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
