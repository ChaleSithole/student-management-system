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
        public async Task<IActionResult> Index(
            string? searchString,
            int? facultyId,
            int? programmeId,
            StudentStatus? status,
            int? yearLevel,
            int? pageNumber)
        {
            const int pageSize = 5;

            var students = await _studentService.GetStudentsAsync(
                searchString,
                facultyId,
                programmeId,
                status,
                yearLevel,
                pageNumber ?? 1,
                pageSize);

            ViewBag.TotalStudents = _studentService.GetTotalStudents();
            ViewBag.ActiveStudents = _studentService.GetStudentsByStatus(StudentStatus.Active);
            ViewBag.GraduatedStudents = _studentService.GetStudentsByStatus(StudentStatus.Graduated);
            ViewBag.SuspendedStudents = _studentService.GetStudentsByStatus(StudentStatus.Suspended);

            ViewBag.Faculties = _studentService.GetAllFaculties();
            ViewBag.Programmes = _studentService.GetAllProgrammes();

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentFaculty = facultyId;
            ViewBag.CurrentProgramme = programmeId;
            ViewBag.CurrentStatus = status;
            ViewBag.CurrentYearLevel = yearLevel;

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
                bool success = _studentService.AddStudent(student);

                if (!success)
                {
                    ModelState.AddModelError(
                        "StudentNumber",
                        "A student with this Student Number already exists.");

                    ViewBag.Faculties = _studentService.GetAllFaculties();
                    ViewBag.Programmes = _studentService.GetAllProgrammes();

                    return View(student);
                }

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
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(string studentNumber)
        {
            _studentService.DeleteStudent(studentNumber);

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

        [HttpGet]
        public JsonResult GetProgrammes(int facultyId)
        {
            var programmes = _studentService
                .GetAllProgrammes()
                .Where(p => p.FacultyId == facultyId)
                .Select(p => new
                {
                    p.ProgrammeId,
                    p.ProgrammeName
                })
                .ToList();

            return Json(programmes);
        }
    }
}
