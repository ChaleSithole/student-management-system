using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class FacultyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}