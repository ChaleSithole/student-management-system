using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;
using StudentManagementSystem.Models.ViewModels;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    // Handles programme management operations.
    [Authorize]
    public class ProgrammeController : Controller
    {
        private readonly IProgrammeService _programmeService;
        private readonly IStudentService _studentService;

        // Injects programme and student services.
        public ProgrammeController(
            IProgrammeService programmeService,
            IStudentService studentService)
        {
            _programmeService = programmeService;
            _studentService = studentService;
        }

        // Displays all programmes and their student counts.
        public IActionResult Index()
        {
            // Displays all programmes and their student counts.
            var programmes = _programmeService.GetAll();

            // Build the ViewModel with programme statistics.
            var model = programmes.Select(p => new ProgrammeListViewModel
            {
                Programme = p,
                StudentCount = _programmeService.GetStudentCount(p.ProgrammeId),
                CanDelete = _programmeService.GetStudentCount(p.ProgrammeId) == 0
            }).ToList();

            // Populate summary cards.
            ViewBag.TotalProgrammes = _programmeService.GetTotalProgrammes();
            ViewBag.TotalStudents = model.Sum(x => x.StudentCount);
            ViewBag.TotalFaculties = _studentService.GetAllFaculties().Count;

            // Display the programme list.
            return View(model);
        }

        // Displays the Add Programme page.
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            // Populate the faculty dropdown.
            ViewBag.Faculties = new SelectList(
                _studentService.GetAllFaculties(),
                "FacultyId",
                "FacultyName");

            return View();
        }

        // Saves a new programme.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Programme programme)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Faculties = new SelectList(
                    _studentService.GetAllFaculties(),
                    "FacultyId",
                    "FacultyName");

                return View(programme);
            }

            // Save the programme.
            _programmeService.Add(programme);

            TempData["SuccessMessage"] =
                "Programme created successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Loads a programme for editing.
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            var programme = _programmeService.GetById(id);

            if (programme == null)
                return NotFound();

            ViewBag.Faculties = new SelectList(
                _studentService.GetAllFaculties(),
                "FacultyId",
                "FacultyName",
                programme.FacultyId);

            return View(programme);
        }

        // Updates the selected programme.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Programme programme)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Faculties = new SelectList(
                    _studentService.GetAllFaculties(),
                    "FacultyId",
                    "FacultyName",
                    programme.FacultyId);

                return View(programme);
            }

            // Updates the selected programme.
            _programmeService.Update(programme);

            TempData["SuccessMessage"] =
                "Programme updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Displays the delete confirmation page.
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            var programme = _programmeService.GetById(id);

            if (programme == null)
                return NotFound();

            return View(programme);
        }

        // Deletes a programme when no students
        // are enrolled.
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Attempt to delete the programme.
            bool deleted = _programmeService.Delete(id);

            if (!deleted)
            {
                TempData["ErrorMessage"] =
                    "Programme cannot be deleted because students are enrolled.";

                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] =
                "Programme deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}