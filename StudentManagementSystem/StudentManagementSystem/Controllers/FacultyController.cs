using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Models.ViewModels;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class FacultyController : Controller
    {
        private readonly IFacultyService _facultyService;

        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        public IActionResult Index()
        {
            var faculties = _facultyService.GetAll();

            var model = faculties.Select(f => new FacultyListViewModel
            {
                Faculty = f,
                ProgrammeCount = _facultyService.GetProgrammeCount(f.FacultyId),
                StudentCount = _facultyService.GetStudentCount(f.FacultyId),
                CanDelete = _facultyService.GetProgrammeCount(f.FacultyId) == 0
            }).ToList();

            ViewBag.TotalFaculties = _facultyService.GetTotalFaculties();
            ViewBag.TotalProgrammes = model.Sum(x => x.ProgrammeCount);
            ViewBag.TotalStudents = model.Sum(x => x.StudentCount);

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Faculty faculty)
        {
            if (!ModelState.IsValid)
                return View(faculty);

            _facultyService.Add(faculty);

            TempData["SuccessMessage"] = "Faculty added successfully.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var faculty = _facultyService.GetById(id);

            if (faculty == null)
                return NotFound();

            return View(faculty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Faculty faculty)
        {
            if (!ModelState.IsValid)
                return View(faculty);

            _facultyService.Update(faculty);

            TempData["SuccessMessage"] = "Faculty updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var faculty = _facultyService.GetById(id);

            if (faculty == null)
                return NotFound();

            return View(faculty);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool deleted = _facultyService.Delete(id);

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