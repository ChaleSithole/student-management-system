using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Models.ViewModels;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    // Handles all faculty management operations.
    [Authorize]
    public class FacultyController : Controller
    {
        private readonly IFacultyService _facultyService;

        // Injects the faculty service.
        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        // Displays all faculties together with
        // programme and student statistics.
        public IActionResult Index()
        {
            // Retrieve every faculty from the database.
            var faculties = _facultyService.GetAll();

            // Build the ViewModel containing additional statistics.
            var model = faculties.Select(f => new FacultyListViewModel
            {
                Faculty = f,
                ProgrammeCount = _facultyService.GetProgrammeCount(f.FacultyId),
                StudentCount = _facultyService.GetStudentCount(f.FacultyId),
                CanDelete = _facultyService.GetProgrammeCount(f.FacultyId) == 0
            }).ToList();

            // Dashboard cards displayed above the table.
            ViewBag.TotalFaculties = _facultyService.GetTotalFaculties();
            ViewBag.TotalProgrammes = model.Sum(x => x.ProgrammeCount);
            ViewBag.TotalStudents = model.Sum(x => x.StudentCount);

            return View(model);
        }

        // Displays the Add Faculty page.
        // Administrators only.
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // Saves a new faculty.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(Faculty faculty)
        {
            // Stop if validation fails.
            if (!ModelState.IsValid)
                return View(faculty);

            // Save the faculty to the database.
            _facultyService.Add(faculty);

            TempData["SuccessMessage"] = "Faculty added successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Loads the selected faculty for editing.
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            var faculty = _facultyService.GetById(id);

            if (faculty == null)
                return NotFound();

            return View(faculty);
        }

        // Updates an existing faculty.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(Faculty faculty)
        {
            if (!ModelState.IsValid)
                return View(faculty);

            // Save the updated faculty.
            _facultyService.Update(faculty);

            TempData["SuccessMessage"] = "Faculty updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Displays the delete confirmation page.
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            var faculty = _facultyService.GetById(id);

            if (faculty == null)
                return NotFound();

            return View(faculty);
        }

        // Deletes a faculty if it contains
        // no programmes.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Attempt to delete the faculty.
            bool deleted = _facultyService.Delete(id);

            // Prevent deletion when programmes still exist.
            if (!deleted)
            {
                TempData["ErrorMessage"] =
                    "This faculty cannot be deleted because it still has programmes.";

                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] =
                "Faculty deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}