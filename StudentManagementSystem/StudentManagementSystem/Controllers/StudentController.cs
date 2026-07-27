using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
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
            string? sortOrder,
            int? pageNumber)
        {
            const int pageSize = 5;

            var students = await _studentService.GetStudentsAsync(
                searchString,
                facultyId,
                programmeId,
                status,
                yearLevel,
                sortOrder,
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
            ViewBag.CurrentSort = sortOrder;

            ViewBag.StudentNumberSort = sortOrder == "number_desc" ? "" : "number_desc";

            ViewBag.NameSort = sortOrder == "name" ? "name_desc" : "name";

            ViewBag.FacultySort = sortOrder == "faculty" ? "faculty_desc" : "faculty";

            ViewBag.ProgrammeSort = sortOrder == "programme" ? "programme_desc" : "programme";

            ViewBag.YearSort = sortOrder == "year" ? "year_desc" : "year";

            ViewBag.StatusSort = sortOrder == "status" ? "status_desc" : "status";

            ViewBag.CurrentSort = sortOrder;

            ViewBag.StudentNumberIndicator =
                sortOrder == "number_desc" ? "▼" : sortOrder == "" || sortOrder == null ? "▲" : "";

            ViewBag.NameIndicator =
                sortOrder == "name" ? "▲" :
                sortOrder == "name_desc" ? "▼" : "";

            ViewBag.FacultyIndicator =
                sortOrder == "faculty" ? "▲" :
                sortOrder == "faculty_desc" ? "▼" : "";

            ViewBag.ProgrammeIndicator =
                sortOrder == "programme" ? "▲" :
                sortOrder == "programme_desc" ? "▼" : "";

            ViewBag.YearIndicator =
                sortOrder == "year" ? "▲" :
                sortOrder == "year_desc" ? "▼" : "";

            ViewBag.StatusIndicator =
                sortOrder == "status" ? "▲" :
                sortOrder == "status_desc" ? "▼" : "";

            return View(students);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewBag.Faculties = _studentService.GetAllFaculties();
            ViewBag.Programmes = _studentService.GetAllProgrammes();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
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

        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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

        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
