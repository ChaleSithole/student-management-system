using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    public interface IStudentService
    {
        Task<PaginatedList<Student>> GetStudentsAsync(int pageIndex, int pageSize);

        Student? GetStudentById(string studentNumber);

        bool AddStudent(Student student);

        void UpdateStudent(Student student);

        void DeleteStudent(string studentNumber);

        int GetTotalStudents();

        int GetActiveStudents();

        int GetGraduatedStudents();

        int GetSuspendedStudents();
        List<Faculty> GetAllFaculties();

        List<Programme> GetAllProgrammes();

        int GetStudentsByStatus(StudentStatus status);
    }
}