using EmployeeManagement.Data.Entities;

namespace EmployeeManagement.Models.Filters;

public class EmployeeFilter
{
    public string EmployeeID { get; set; }
    public string Name { get; set; }
    public int DepartmentID { get; set; }
    public DateTime? RegistrationDate { get; set; }
}