using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        public IActionResult Index(string searchString)
        {
            var students = _studentService.GetAllStudents();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                students = students.Where(s =>
                    s.StudentNumber.Contains(searchString) ||
                    s.FirstName.Contains(searchString) ||
                    s.LastName.Contains(searchString))
                    .ToList();
            }

            ViewBag.TotalStudents = _studentService.GetTotalStudents();
            ViewBag.ActiveStudents = _studentService.GetActiveStudents();
            ViewBag.GraduatedStudents = _studentService.GetGraduatedStudents();
            ViewBag.SuspendedStudents = _studentService.GetSuspendedStudents();

            return View(students);
        }

        public IActionResult Create()
        {
            ViewBag.Faculties = _studentService.GetAllFaculties();
            ViewBag.Programmes = _studentService.GetAllProgrammes();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _studentService.AddStudent(student);

                TempData["SuccessMessage"] = "Student created successfully.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Faculties = _studentService.GetAllFaculties();
            ViewBag.Programmes = _studentService.GetAllProgrammes();

            return View(student);
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
                return NotFound();

            var student = _studentService.GetStudentById(id);

            if (student == null)
                return NotFound();

            ViewBag.Faculties = _studentService.GetAllFaculties();
            ViewBag.Programmes = _studentService.GetAllProgrammes();

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                _studentService.UpdateStudent(student);

                TempData["SuccessMessage"] = "Student updated successfully.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Faculties = _studentService.GetAllFaculties();
            ViewBag.Programmes = _studentService.GetAllProgrammes();

            return View(student);
        }

        public IActionResult Delete(string id)
        {
            if (id == null)
                return NotFound();

            var student = _studentService.GetStudentById(id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Student student)
        {
            _studentService.DeleteStudent(student.StudentNumber);

            TempData["SuccessMessage"] = "Student deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(string id)
        {
            if (id == null)
                return NotFound();

            var student = _studentService.GetStudentById(id);

            if (student == null)
                return NotFound();

            return View(student);
        }
    }
}
