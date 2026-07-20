using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    public class FacultyService : IFacultyService
    {
        private readonly ApplicationDbContext _context;

        public FacultyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Faculty> GetAll()
        {
            return _context.Faculties
                .OrderBy(f => f.FacultyName)
                .ToList();
        }

        public Faculty? GetById(int id)
        {
            return _context.Faculties.Find(id);
        }

        public void Add(Faculty faculty)
        {
            _context.Faculties.Add(faculty);
            _context.SaveChanges();
        }

        public void Update(Faculty faculty)
        {
            var existing = _context.Faculties.Find(faculty.FacultyId);

            if (existing == null)
                return;

            existing.FacultyName = faculty.FacultyName;

            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var faculty = _context.Faculties.Find(id);

            if (faculty == null)
                return false;

            if (_context.Programmes.Any(p => p.FacultyId == id))
                return false;

            _context.Faculties.Remove(faculty);

            _context.SaveChanges();

            return true;
        }

        public int GetProgrammeCount(int facultyId)
        {
            return _context.Programmes
                .Count(p => p.FacultyId == facultyId);
        }

        public int GetStudentCount(int facultyId)
        {
            return _context.Students
                .Count(s => s.FacultyId == facultyId);
        }

        public int GetTotalFaculties()
        {
            return _context.Faculties.Count();
        }
    }
}