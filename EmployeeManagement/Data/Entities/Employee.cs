namespace EmployeeManagement.Data.Entities;

public class Employee
{
    public string EmployeeID { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; } = string.Empty;
    public int PositionID { get; set; }
    public Position Position { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string RecommendationLetter { get; set; }
    public string EmployeePhoto { get; set; }
    public string NrcFrontImage { get; set; }
    public string NrcBackImage { get; set; }
    public bool IsRecordDeleted { get; set; } = false;
}