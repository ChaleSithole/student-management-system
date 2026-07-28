using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    // Represents an academic programme.
    public class Programme
    {
        [Key]
        public int ProgrammeId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProgrammeName { get; set; } = string.Empty;

        // Foreign Key
        public int FacultyId { get; set; }

        // Navigation Property
        public Faculty? Faculty { get; set; }

        // One Programme has many Students
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}