using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    // Provides business logic for faculty management.
    public class FacultyService : IFacultyService
    {
        private readonly ApplicationDbContext _context;

        public FacultyService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieves every faculty.
        public List<Faculty> GetAll()
        {
            // Retrieve all faculties including related data.
            return _context.Faculties
                .OrderBy(f => f.FacultyName)
                .ToList();
        }

        public Faculty? GetById(int id)
        {
            return _context.Faculties.Find(id);
        }

        //Creates a new faculty and saves it to the database.
        public void Add(Faculty faculty)
        {
            _context.Faculties.Add(faculty);
            _context.SaveChanges();
        }

        //Updates an existing faculty in the database.
        public void Update(Faculty faculty)
        {
            var existing = _context.Faculties.Find(faculty.FacultyId);

            if (existing == null)
                return;

            existing.FacultyName = faculty.FacultyName;

            _context.SaveChanges();
        }
        //Deletes a faculty from the database if it has no associated programmes.
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

        //Counts the number of programmes associated with a specific faculty.
        public int GetProgrammeCount(int facultyId)
        {
            return _context.Programmes
                .Count(p => p.FacultyId == facultyId);
        }

        //Counts the number of students associated with a specific faculty. 
        public int GetStudentCount(int facultyId)
        {
            return _context.Students
                .Count(s => s.FacultyId == facultyId);
        }

        //Counts the total number of faculties in the database.
        public int GetTotalFaculties()
        {
            return _context.Faculties.Count();
        }
    }
}