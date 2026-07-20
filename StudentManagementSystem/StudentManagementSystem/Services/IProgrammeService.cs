using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    public interface IProgrammeService
    {
        List<Programme> GetAll();

        Programme? GetById(int id);

        void Add(Programme programme);

        void Update(Programme programme);

        bool Delete(int id);

        int GetStudentCount(int programmeId);

        int GetTotalProgrammes();
    }
}