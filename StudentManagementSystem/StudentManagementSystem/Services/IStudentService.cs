using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    public interface IStudentService
    {
        List<Student> GetAllStudents();

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
    }
}