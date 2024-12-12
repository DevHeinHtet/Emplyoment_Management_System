using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Data.Entities;
public class Department
{
    public int DepartmentID { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Position> Positions { get; set; } = new List<Position>();
}