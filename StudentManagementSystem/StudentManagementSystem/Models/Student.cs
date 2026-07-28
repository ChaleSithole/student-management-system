using StudentManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

public class Student : IValidatableObject
{
    // Represents a university student.
    // Stores personal information, academic details
    // and relationships to Faculty and Programme.
   [Key]
    [Required(ErrorMessage = "Student Number is required.")]
    [StringLength(10, ErrorMessage = "Student Number cannot exceed 10 characters.")]
    [Display(Name = "Student Number")]
    public string StudentNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "First Name is required.")]
    [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last Name is required.")]
    [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email Address is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    public StudentStatus Status { get; set; } = StudentStatus.Active;

    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [StringLength(10)]
    [Display(Name = "Phone Number")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Date of Birth is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Please select a Faculty.")]
    [Display(Name = "Faculty")]
    public int FacultyId { get; set; }

    public Faculty? Faculty { get; set; }

    [Required(ErrorMessage = "Please select a Programme.")]
    [Display(Name = "Programme")]
    public int ProgrammeId { get; set; }

    public Programme? Programme { get; set; }

    [Required(ErrorMessage = "Year Level is required.")]
    [Range(1, 6, ErrorMessage = "Year Level must be between 1 and 6.")]
    [Display(Name = "Year Level")]
    public int YearLevel { get; set; }

    [Display(Name = "Registration Date")]
    [DataType(DataType.Date)]
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DateOfBirth > DateTime.Today)
        {
            yield return new ValidationResult(
                "Date of Birth cannot be in the future.",
                new[] { nameof(DateOfBirth) });
        }

        var age = DateTime.Today.Year - DateOfBirth.Year;

        if (DateOfBirth.Date > DateTime.Today.AddYears(-age))
        {
            age--;
        }

        if (age < 15)
        {
            yield return new ValidationResult(
                "Student must be at least 15 years old.",
                new[] { nameof(DateOfBirth) });
        }

        if (age > 100)
        {
            yield return new ValidationResult(
                "Please enter a valid Date of Birth.",
                new[] { nameof(DateOfBirth) });
        }
    }
}