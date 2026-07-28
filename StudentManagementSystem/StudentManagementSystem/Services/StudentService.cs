using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    // Provides all business logic related to students.
    // Responsible for communicating with the database
    // through Entity Framework.
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        // Injects the application's database context.
        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Adds a new student after checking for duplicate student numbers.
        public bool AddStudent(Student student)
        {
            // Ensure the student number is unique.
            bool studentExists = _context.Students
                .Any(s => s.StudentNumber == student.StudentNumber);

            if (studentExists)
            {
                return false;
            }

            // Save the new student.
            _context.Students.Add(student);
            _context.SaveChanges();

            return true;
        }

        // Deletes the selected student.
        public void DeleteStudent(string studentNumber)
        {
            // Find the student to remove.
            var student = _context.Students.Find(studentNumber);

            if (student == null)
            {
                return;
            }

            // Remove the student from the database.
            _context.Students.Remove(student);

            _context.SaveChanges();
        }

        // Retrieves students after applying searching, filtering, sorting and pagination.
        public async Task<PaginatedList<Student>> GetStudentsAsync(
                string? searchString,
                int? facultyId,
                int? programmeId,
                StudentStatus? status,
                int? yearLevel,
                string? sortOrder,
                int pageIndex,
                int pageSize)
        {
            var students = _context.Students.AsNoTracking()
                .Include(s => s.Faculty)
                .Include(s => s.Programme)
                .AsQueryable();
            
            // Apply search filtering.
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                students = students.Where(s =>
                    s.StudentNumber.Contains(searchString) ||
                    s.FirstName.Contains(searchString) ||
                    s.LastName.Contains(searchString));
            }

            // Filter by faculty.
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

            // Apply the selected sorting option.
            switch (sortOrder)
            {
                case "number_desc":
                    students = students.OrderByDescending(s => s.StudentNumber);
                    break;

                case "name":
                    students = students.OrderBy(s => s.FirstName)
                                       .ThenBy(s => s.LastName);
                    break;

                case "name_desc":
                    students = students.OrderByDescending(s => s.FirstName)
                                       .ThenByDescending(s => s.LastName);
                    break;

                case "faculty":
                    students = students.OrderBy(s => s.Faculty!.FacultyName);
                    break;

                case "faculty_desc":
                    students = students.OrderByDescending(s => s.Faculty!.FacultyName);
                    break;

                case "programme":
                    students = students.OrderBy(s => s.Programme!.ProgrammeName);
                    break;

                case "programme_desc":
                    students = students.OrderByDescending(s => s.Programme!.ProgrammeName);
                    break;

                case "year":
                    students = students.OrderBy(s => s.YearLevel);
                    break;

                case "year_desc":
                    students = students.OrderByDescending(s => s.YearLevel);
                    break;

                case "status":
                    students = students.OrderBy(s => s.Status);
                    break;

                case "status_desc":
                    students = students.OrderByDescending(s => s.Status);
                    break;

                default:
                    students = students.OrderBy(s => s.StudentNumber);
                    break;
            }

            // Return only the requested page of students.
            return await PaginatedList<Student>.CreateAsync(
                students,
                pageIndex,
                pageSize);
        }

        // Returns the total number of students.
        public int GetTotalStudents()
        {
            return _context.Students.Count();
        }

        // Counts students belonging to a specific status.
        public int GetStudentsByStatus(StudentStatus status)
        {
            return _context.Students.Count(s => s.Status == status);
        }

        // Retrieves a single student using their student number.
        public Student? GetStudentById(string studentNumber)
        {
            return _context.Students.AsNoTracking()
                .Include(s => s.Faculty)
                .Include(s => s.Programme)
                .FirstOrDefault(s => s.StudentNumber == studentNumber);
        }

        // Updates an existing student's information.
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

            // Commit the changes.
            _context.SaveChanges();
        }

        // Retrieves every faculty for dropdown lists.
        public List<Faculty> GetAllFaculties()
        {
            return _context.Faculties.AsNoTracking()
                .OrderBy(f => f.FacultyName)
                .ToList();
        }

        // Retrieves every programme for dropdown lists.
        public List<Programme> GetAllProgrammes()
        {
            return _context.Programmes.AsNoTracking()
                .OrderBy(p => p.ProgrammeName)
                .ToList();
        }

        
       
    }
}