using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
    public byte[] RecommendationLetter { get; set; } // PDF file as byte[]
    public byte[] EmployeePhoto { get; set; } // JPG file as byte[]
    public byte[] NrcFrontImage { get; set; } // Front NRC image as byte[]
    public byte[] NrcBackImage { get; set; } // Back NRC image as byte[]
    public bool IsRecordDeleted { get; set; } = false;
}
