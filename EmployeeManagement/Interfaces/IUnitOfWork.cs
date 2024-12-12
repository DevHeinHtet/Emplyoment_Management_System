namespace EmployeeManagement.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IPositionRepository Positions { get; }
    IEmployeeRepository Employees { get; }
    IDepartmentRepository Departments { get; }
    Task<int> SaveChangesAsync();
}