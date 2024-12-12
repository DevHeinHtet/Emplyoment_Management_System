using EmployeeManagement.Data.Entities;

namespace EmployeeManagement.Interfaces;

public interface IPositionRepository : IGenericRepository<Position>
{
    Task<IEnumerable<Position>> GetAllPositionsWithDepartmentAsync();
    Task<IEnumerable<Position>> GetPositionsByDepartment(int departmentID);
}
