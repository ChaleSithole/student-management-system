using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    public interface IFacultyService
    {
        List<Faculty> GetAll();

        Faculty? GetById(int id);

        void Add(Faculty faculty);

        void Update(Faculty faculty);

        bool Delete(int id);

        int GetProgrammeCount(int facultyId);

        int GetStudentCount(int facultyId);

        int GetTotalFaculties();
    }
}