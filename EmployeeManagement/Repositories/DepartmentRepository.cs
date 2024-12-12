using EmployeeManagement.Data;
using EmployeeManagement.Data.Entities;
using EmployeeManagement.Interfaces;

namespace EmployeeManagement.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly EmployeeDbContext _context;

        public DepartmentRepository(EmployeeDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
