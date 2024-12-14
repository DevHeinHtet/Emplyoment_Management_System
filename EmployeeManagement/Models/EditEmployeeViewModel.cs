using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Data.Entities;

namespace EmployeeManagement.Models;

public class EditEmployeeViewModel
{
    [Required]
    public string EmployeeID { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string EmployeeName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(EmployeeRegistrationViewModel), nameof(ValidateAge))]
    public DateTime BirthDate { get; set; }

    [Required]
    [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either Male or Female")]
    public string Gender { get; set; }

    public IEnumerable<Department> Departments { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "DepartmentID field is required.")]
    public int DepartmentID { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "PositionID field is required.")]
    public int PositionID { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    public string EmployeePhoto { get; set; }
    public string NrcFrontImage { get; set; }
    public string NrcBackImage { get; set; }
    public IFormFile Profile { get; set; }
    public IFormFile NrcFront { get; set; }
    public IFormFile NrcBack { get; set; }

    public IFormFile RecommendationLetter { get; set; }

    public static ValidationResult ValidateAge(DateTime birthDate, ValidationContext context)
    {
        var age = DateTime.Today.Year - birthDate.Year;
        if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;

        if (age < 20 || age > 45)
        {
            return new ValidationResult("Employee must be between 20 and 45 years old.");
        }

        return ValidationResult.Success;
    }
}
