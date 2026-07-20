using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    public class ProgrammeService : IProgrammeService
    {
        private readonly ApplicationDbContext _context;

        public ProgrammeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Programme> GetAll()
        {
            return _context.Programmes
                .Include(p => p.Faculty)
                .OrderBy(p => p.ProgrammeName)
                .ToList();
        }

        public Programme? GetById(int id)
        {
            return _context.Programmes.Find(id);
        }

        public void Add(Programme programme)
        {
            _context.Programmes.Add(programme);
            _context.SaveChanges();
        }

        public void Update(Programme programme)
        {
            var existing = _context.Programmes.Find(programme.ProgrammeId);

            if (existing == null)
                return;

            existing.ProgrammeName = programme.ProgrammeName;
            existing.FacultyId = programme.FacultyId;

            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var programme = _context.Programmes.Find(id);

            if (programme == null)
                return false;

            if (_context.Students.Any(s => s.ProgrammeId == id))
                return false;

            _context.Programmes.Remove(programme);

            _context.SaveChanges();

            return true;
        }

        public int GetStudentCount(int programmeId)
        {
            return _context.Students
                .Count(s => s.ProgrammeId == programmeId);
        }

        public int GetTotalProgrammes()
        {
            return _context.Programmes.Count();
        }
    }
}