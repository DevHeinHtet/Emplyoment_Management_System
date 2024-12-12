using EmployeeManagement.Data.Entities;

namespace EmployeeManagement.Models
{
    public class CreatePositionViewModel
    {
        public IEnumerable<Department> Departments { get; set; }
        public int DepartmentID { get; set; }
        public string PositionName { get; set; }
    }
}
