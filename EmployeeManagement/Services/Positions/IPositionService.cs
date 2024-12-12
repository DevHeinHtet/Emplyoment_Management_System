using EmployeeManagement.Data.Entities;

namespace EmployeeManagement.Services.Positions;

public interface IPositionService
{
    Task<IEnumerable<Position>> GetAllPositionsWithDepartmentAsync(); 
    Task<IEnumerable<Position>> GetPositionsByDepartment(int departmentID); 
    Task<Position> GetPositionByIdAsync(int id);
    Task AddPositionAsync(Position position);
    Task UpdatePositionAsync(Position position);
    Task DeletePositionAsync(int id);
}