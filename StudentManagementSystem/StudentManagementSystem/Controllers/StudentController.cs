using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagementSystem.Controllers
{
    
    // Handles all student-related operations such as
    // viewing, creating, editing, deleting and searching students.
    
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        // Injects the student service using Dependency Injection.
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // Displays all students and does searching, filtering, sorting & pagnation.
        public async Task<IActionResult> Index(
            string? searchString,
            int? facultyId,
            int? programmeId,
            StudentStatus? status,
            int? yearLevel,
            string? sortOrder,
            int? pageNumber)
        {

            // Number of students displayed per page.
            const int pageSize = 5;

            // Retrieve students after applying all filters and sorting.
            var students = await _studentService.GetStudentsAsync(
                searchString,
                facultyId,
                programmeId,
                status,
                yearLevel,
                sortOrder,
                pageNumber ?? 1,
                pageSize);


            // Dashboard statistics displayed above the student table.
            ViewBag.TotalStudents = _studentService.GetTotalStudents();
            ViewBag.ActiveStudents = _studentService.GetStudentsByStatus(StudentStatus.Active);
            ViewBag.GraduatedStudents = _studentService.GetStudentsByStatus(StudentStatus.Graduated);
            ViewBag.SuspendedStudents = _studentService.GetStudentsByStatus(StudentStatus.Suspended);

            // Populate dropdown lists used by filters.
            ViewBag.Faculties = _studentService.GetAllFaculties();
            ViewBag.Programmes = _studentService.GetAllProgrammes();

            // Preserve selected filter values after each search.
            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentFaculty = facultyId;
            ViewBag.CurrentProgramme = programmeId;
            ViewBag.CurrentStatus = status;
            ViewBag.CurrentYearLevel = yearLevel;
            ViewBag.CurrentSort = sortOrder;

            // Configure ascending and descending sorting for each column.
            ViewBag.StudentNumberSort = sortOrder == "number_desc" ? "" : "number_desc";

            ViewBag.NameSort = sortOrder == "name" ? "name_desc" : "name";

            ViewBag.FacultySort = sortOrder == "faculty" ? "faculty_desc" : "faculty";

            ViewBag.ProgrammeSort = sortOrder == "programme" ? "programme_desc" : "programme";

            ViewBag.YearSort = sortOrder == "year" ? "year_desc" : "year";

            ViewBag.StatusSort = sortOrder == "status" ? "status_desc" : "status";

            ViewBag.CurrentSort = sortOrder;

            // Display ▲ or ▼ icons beside the active sorted column.
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

            // Send the filtered student list to the view.
            return View(students);
        }

        
        // Displays the Create Student page.
        // Only administrators are allowed to access this page.
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            // Populate dropdown lists.
            ViewBag.Faculties = _studentService.GetAllFaculties();
            ViewBag.Programmes = _studentService.GetAllProgrammes();

            return View();
        }

        // Saves a new student to the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(Student student)
        {
            // Ensure all validation rules have passed.
            if (ModelState.IsValid)
            {
                // Prevent duplicate student numbers.
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

        // Loads the selected student into the edit form.
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(string id)
        {
            if (id == null)
                return NotFound();

            // Retrieve the student by student number.
            var student = _studentService.GetStudentById(id);

            if (student == null)
                return NotFound();

            ViewBag.Faculties = _studentService.GetAllFaculties();
            ViewBag.Programmes = _studentService.GetAllProgrammes();

            return View(student);
        }

        // Updates an existing student's information.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                // Save the updated student details.
                _studentService.UpdateStudent(student);

                TempData["SuccessMessage"] = "Student updated successfully.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Faculties = _studentService.GetAllFaculties();
            ViewBag.Programmes = _studentService.GetAllProgrammes();

            return View(student);
        }

        // Displays the delete confirmation page.
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

        // Permanently removes a student from the database.
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

        // Displays detailed information about one student.
        public IActionResult Details(string id)
        {
            if (id == null)
                return NotFound();

            var student = _studentService.GetStudentById(id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // Returns all programmes belonging to the selected faculty.
        // Used by AJAX to update the Programme dropdown.
        [HttpGet]
        public JsonResult GetProgrammes(int facultyId)
        {
            // Retrieve only programmes linked to the selected faculty.
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
