using EmployeeManagement.Data;
using EmployeeManagement.Data.Entities;
using EmployeeManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repositories;

public class PositionRepository : GenericRepository<Position>, IPositionRepository
{
    private readonly EmployeeDbContext _context;

    public PositionRepository(EmployeeDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Position>> GetAllPositionsWithDepartmentAsync()
    {
        return await _context.Positions.Include(x => x.Department).ToListAsync();
    }

    public async Task<IEnumerable<Position>> GetPositionsByDepartment(int departmentID)
    {
        return await _context.Positions.Where(x => x.DepartmentID == departmentID).ToListAsync();
    }
}