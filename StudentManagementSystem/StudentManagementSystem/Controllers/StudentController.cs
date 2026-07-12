using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString)
        {
            var students = _context.Students
                       .Include(s => s.Faculty)
                       .Include(s => s.Programme)
                       .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                students = students.Where(s =>
                    s.StudentNumber.Contains(searchString) ||
                    s.FirstName.Contains(searchString) ||
                    s.LastName.Contains(searchString));
            }

            var allStudents = _context.Students.ToList();

            ViewBag.TotalStudents = allStudents.Count;

            ViewBag.ActiveStudents = allStudents.Count(s => s.Status == StudentStatus.Active);

            ViewBag.GraduatedStudents = allStudents.Count(s => s.Status == StudentStatus.Graduated);

            ViewBag.SuspendedStudents = allStudents.Count(s => s.Status == StudentStatus.Suspended);

            return View(students.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Faculties = _context.Faculties.ToList();

            ViewBag.Programmes = _context.Programmes.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Student created successfully.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Faculties = _context.Faculties.ToList();
            ViewBag.Programmes = _context.Programmes.ToList();

            return View(student);
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _context.Students
                .Include(s => s.Faculty)
                .Include(s => s.Programme)
                .FirstOrDefault(s => s.StudentNumber == id);

            if (student == null)
            {
                return NotFound();
            }

            ViewBag.Faculties = _context.Faculties.ToList();
            ViewBag.Programmes = _context.Programmes.ToList();

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Faculties = _context.Faculties.ToList();
                ViewBag.Programmes = _context.Programmes.ToList();

                return View(student);
            }

            var existingStudent = _context.Students.Find(student.StudentNumber);

            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Email = student.Email;
            existingStudent.FacultyId = student.FacultyId;
            existingStudent.ProgrammeId = student.ProgrammeId;
            existingStudent.YearLevel = student.YearLevel;
            existingStudent.Status = student.Status;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Student updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _context.Students
                .Include(s => s.Faculty)
                .Include(s => s.Programme)
                .FirstOrDefault(s => s.StudentNumber == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Student studentNumber)
        {
            var student = _context.Students.Find(studentNumber);

            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Student deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _context.Students
                .Include(s => s.Faculty)
                .Include(s => s.Programme)
                .FirstOrDefault(s => s.StudentNumber == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
    }
}
