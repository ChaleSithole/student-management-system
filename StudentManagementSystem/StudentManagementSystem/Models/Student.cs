using StudentManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

public class Student
{
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
    public string? Phone { get; set; }

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
}