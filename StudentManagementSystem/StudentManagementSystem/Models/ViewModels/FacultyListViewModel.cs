using StudentManagementSystem.Models;

namespace StudentManagementSystem.Models.ViewModels
{
    public class FacultyListViewModel
    {
        public Faculty Faculty { get; set; } = null!;

        public int ProgrammeCount { get; set; }

        public int StudentCount { get; set; }

        public bool CanDelete { get; set; }
    }
}