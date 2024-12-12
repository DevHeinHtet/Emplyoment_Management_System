using EmployeeManagement.Data.Entities;

namespace EmployeeManagement.Models;

public class EmployeeViewModel
{
    public string EmployeeID { get; set; }  
    public string Name { get; set; }
    public int DepartmentID { get; set; } 
    public DateTime? RegistrationDate { get; set; }

    public IEnumerable<Department> Departments { get; set; }
    public IEnumerable<Employee> Employees { get; set; }
}