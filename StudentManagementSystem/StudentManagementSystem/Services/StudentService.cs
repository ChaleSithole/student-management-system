using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddStudent(Student student)
        {
            bool studentExists = _context.Students
                .Any(s => s.StudentNumber == student.StudentNumber);

            if (studentExists)
            {
                return false;
            }

            _context.Students.Add(student);
            _context.SaveChanges();

            return true;
        }
        public void DeleteStudent(string studentNumber)
        {
            var student = _context.Students.Find(studentNumber);

            if (student == null)
            {
                return;
            }

            _context.Students.Remove(student);

            _context.SaveChanges();
        }

        public async Task<PaginatedList<Student>> GetStudentsAsync(string? searchString,
                int? facultyId, int? programmeId,StudentStatus? status,int? yearLevel,
                int pageIndex,int pageSize)
        {
            var students = _context.Students.AsNoTracking()
                .Include(s => s.Faculty)
                .Include(s => s.Programme)
                .OrderBy(s => s.StudentNumber)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                students = students.Where(s =>
                    s.StudentNumber.Contains(searchString) ||
                    s.FirstName.Contains(searchString) ||
                    s.LastName.Contains(searchString));
            }

            if (facultyId.HasValue)
            {
                students = students.Where(s => s.FacultyId == facultyId.Value);
            }

            if (programmeId.HasValue)
            {
                students = students.Where(s => s.ProgrammeId == programmeId.Value);
            }

            if (status.HasValue)
            {
                students = students.Where(s => s.Status == status.Value);
            }

            if (yearLevel.HasValue)
            {
                students = students.Where(s => s.YearLevel == yearLevel.Value);
            }
            

            return await PaginatedList<Student>.CreateAsync(
                students,
                pageIndex,
                pageSize);
        }

        public int GetTotalStudents()
        {
            return _context.Students.Count();
        }

        public int GetStudentsByStatus(StudentStatus status)
        {
            return _context.Students.Count(s => s.Status == status);
        }

        public Student? GetStudentById(string studentNumber)
        {
            return _context.Students.AsNoTracking()
                .Include(s => s.Faculty)
                .Include(s => s.Programme)
                .FirstOrDefault(s => s.StudentNumber == studentNumber);
        }

        public void UpdateStudent(Student student)
        {
            var existingStudent = _context.Students.Find(student.StudentNumber);

            if (existingStudent == null)
            {
                return;
            }

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Email = student.Email;
            existingStudent.Phone = student.Phone;
            existingStudent.DateOfBirth = student.DateOfBirth;
            existingStudent.FacultyId = student.FacultyId;
            existingStudent.ProgrammeId = student.ProgrammeId;
            existingStudent.YearLevel = student.YearLevel;
            existingStudent.Status = student.Status;

            _context.SaveChanges();
        }

        public List<Faculty> GetAllFaculties()
        {
            return _context.Faculties.AsNoTracking()
                .OrderBy(f => f.FacultyName)
                .ToList();
        }

        public List<Programme> GetAllProgrammes()
        {
            return _context.Programmes.AsNoTracking()
                .OrderBy(p => p.ProgrammeName)
                .ToList();
        }

        
       
    }
}