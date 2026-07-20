using StudentManagementSystem.Models;

namespace StudentManagementSystem.Models.ViewModels
{
    public class ProgrammeListViewModel
    {
        public Programme Programme { get; set; } = null!;

        public int StudentCount { get; set; }

        public bool CanDelete { get; set; }
    }
}