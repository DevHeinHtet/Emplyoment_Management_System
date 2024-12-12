using EmployeeManagement.Data;
using EmployeeManagement.Data.Entities;
using EmployeeManagement.Interfaces;
using EmployeeManagement.Models.Filters;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repositories;

public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
    private readonly EmployeeDbContext _context;

    public EmployeeRepository(EmployeeDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Employee> GetLatestEmployee()
    {
        return await _context.Employees
            .OrderByDescending(x => x.EmployeeID)
            .FirstOrDefaultAsync(x => x.IsRecordDeleted == false);
    }

    public async Task<IEnumerable<Employee>> GetEmployeesWithIncludeAsync(EmployeeFilter filter)
    {
        var query = _context.Employees
            .Where(e => e.IsRecordDeleted == false);

        if (!string.IsNullOrWhiteSpace(filter.EmployeeID))
        {
            query = query.Where(e => e.EmployeeID.Contains(filter.EmployeeID));
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(e => e.EmployeeName.Contains(filter.Name));
        }

        if (filter.DepartmentID > 0)
        {
            query = query.Where(e => e.Position.DepartmentID == filter.DepartmentID);
        }

        if (filter.RegistrationDate.HasValue)
        {
            query = query.Where(e => e.RegistrationDate == filter.RegistrationDate);
        }

        query = query.Include(e => e.Position)
                    .ThenInclude(p => p.Department);

        return await query.ToListAsync();
    }

    public async Task<Employee> GetEmployeeByEmployeeIDAsync(string employeeID)
    {
        return await _context.Employees
            .Where(x => x.EmployeeID == employeeID && x.IsRecordDeleted == false)
            .Include(e => e.Position)
                .ThenInclude(p => p.Department)
            .FirstOrDefaultAsync();
    }
}