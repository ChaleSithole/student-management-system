using StudentManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

public class Student
{
    [Key]
    [Required]
    [StringLength(15)]
    public string StudentNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public StudentStatus Status { get; set; } = StudentStatus.Active;
    public string? Phone { get; set; }

    public DateTime DateOfBirth { get; set; }

    [Required]
    public string Faculty { get; set; } = string.Empty;

    [Required]
    public string Programme { get; set; } = string.Empty;

    [Range(1, 6)]
    public int YearLevel { get; set; }

    public DateTime RegistrationDate { get; set; } = DateTime.Now;
}