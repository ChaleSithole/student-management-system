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
                       .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                students = students.Where(s =>
                    s.StudentNumber.Contains(searchString) ||
                    s.FirstName.Contains(searchString) ||
                    s.LastName.Contains(searchString));
            }

            ViewBag.TotalStudents = _context.Students.Count();

            ViewBag.ActiveStudents = _context.Students.Count(s => s.Status == StudentStatus.Active);

            ViewBag.GraduatedStudents = _context.Students.Count(s => s.Status == StudentStatus.Graduated);

            ViewBag.SuspendedStudents = _context.Students.Count(s => s.Status == StudentStatus.Suspended);

            return View(students.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Faculties = _context.Faculties.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Faculties = _context.Faculties.ToList();

            return View(student);
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _context.Students.Find(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Update(student);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }

        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _context.Students.Find(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Student student)
        {
            var studentToDelete = _context.Students.Find(student.StudentNumber);

            if (studentToDelete == null)
            {
                return NotFound();
            }

            _context.Students.Remove(studentToDelete);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _context.Students.Find(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
    }
}
