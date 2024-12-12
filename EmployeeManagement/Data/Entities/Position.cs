using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Data.Entities;

public class Position
{
    public int PositionID { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DepartmentID { get; set; }
    public Department Department { get; set; }
}