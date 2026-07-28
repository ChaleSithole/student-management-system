using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    // Represents a university faculty.
    public class Faculty
    {
        [Key]
        public int FacultyId { get; set; }

        [Required]
        [StringLength(100)]
        public string FacultyName { get; set; } = string.Empty;

        // One Faculty has many Students
        public ICollection<Student> Students { get; set; } = new List<Student>();

        public ICollection<Programme> Programmes { get; set; } = new List<Programme>();
    }
}