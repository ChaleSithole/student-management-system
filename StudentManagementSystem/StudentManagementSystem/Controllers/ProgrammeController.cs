using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;
using StudentManagementSystem.Models.ViewModels;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class ProgrammeController : Controller
    {
        private readonly IProgrammeService _programmeService;
        private readonly IStudentService _studentService;

        public ProgrammeController(
            IProgrammeService programmeService,
            IStudentService studentService)
        {
            _programmeService = programmeService;
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            var programmes = _programmeService.GetAll();

            var model = programmes.Select(p => new ProgrammeListViewModel
            {
                Programme = p,
                StudentCount = _programmeService.GetStudentCount(p.ProgrammeId),
                CanDelete = _programmeService.GetStudentCount(p.ProgrammeId) == 0
            }).ToList();

            ViewBag.TotalProgrammes = _programmeService.GetTotalProgrammes();
            ViewBag.TotalStudents = model.Sum(x => x.StudentCount);
            ViewBag.TotalFaculties = _studentService.GetAllFaculties().Count;

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Faculties = new SelectList(
                _studentService.GetAllFaculties(),
                "FacultyId",
                "FacultyName");

            return View();
        }

        [HttpPost]
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

            _programmeService.Add(programme);

            TempData["SuccessMessage"] =
                "Programme created successfully.";

            return RedirectToAction(nameof(Index));
        }

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

        [HttpPost]
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

            _programmeService.Update(programme);

            TempData["SuccessMessage"] =
                "Programme updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var programme = _programmeService.GetById(id);

            if (programme == null)
                return NotFound();

            return View(programme);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
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