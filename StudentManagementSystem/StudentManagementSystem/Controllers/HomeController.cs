using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Models.ViewModels;
using StudentManagementSystem.Services;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagementSystem.Controllers
{
    // Displays the application dashboard and general pages.
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStudentService _studentService;
        private readonly IFacultyService _facultyService;
        private readonly IProgrammeService _programmeService;

        // Injects the services required to build the dashboard.
        public HomeController(
            ILogger<HomeController> logger,
            IStudentService studentService,
            IFacultyService facultyService,
            IProgrammeService programmeService)
        {
            _logger = logger;
            _studentService = studentService;
            _facultyService = facultyService;
            _programmeService = programmeService;
        }

        // Displays the dashboard containing overall statistics
        // about students, faculties and programmes.
        public IActionResult Index()
        {
            // Create the dashboard model that contains all summary statistics.
            DashboardViewModel model = new DashboardViewModel
            {
                // Retrieve overall totals from the service layer.
                TotalStudents = _studentService.GetTotalStudents(),
                ActiveStudents = _studentService.GetStudentsByStatus(StudentStatus.Active),
                TotalFaculties = _facultyService.GetTotalFaculties(),
                TotalProgrammes = _programmeService.GetTotalProgrammes()
            };

            // Pass the dashboard data to the view.
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ??
                            HttpContext.TraceIdentifier
            });
        }
    }
}