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

        public int GetActiveStudents()
        {
            return _context.Students.Count(s => s.Status == StudentStatus.Active);
        }

        public List<Student> GetAllStudents()
        {
            return _context.Students
                .Include(s => s.Faculty)
                .Include(s => s.Programme)
                .ToList();
        }

        public int GetGraduatedStudents()
        {
            return _context.Students.Count(s => s.Status == StudentStatus.Graduated);
        }

        public Student? GetStudentById(string studentNumber)
        {
            return _context.Students
                .Include(s => s.Faculty)
                .Include(s => s.Programme)
                .FirstOrDefault(s => s.StudentNumber == studentNumber);
        }

        public int GetSuspendedStudents()
        {
            return _context.Students.Count(s => s.Status == StudentStatus.Suspended);
        }

        public int GetTotalStudents()
        {
            return _context.Students.Count();
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
            existingStudent.FacultyId = student.FacultyId;
            existingStudent.ProgrammeId = student.ProgrammeId;
            existingStudent.YearLevel = student.YearLevel;
            existingStudent.Status = student.Status;

            _context.SaveChanges();
        }

        public List<Faculty> GetAllFaculties()
        {
            return _context.Faculties.ToList();
        }

        public List<Programme> GetAllProgrammes()
        {
            return _context.Programmes
                .Include(p => p.Faculty)
                .ToList();
        }
    }
}