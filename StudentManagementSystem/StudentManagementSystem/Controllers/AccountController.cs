using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagementSystem.Controllers
{
    // Handles user authentication, login and logout.
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        // Injects ASP.NET Identity services.
        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Displays the login page.
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // Authenticates the user using ASP.NET Core Identity.
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Stop if validation fails.
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                false);

            // Redirect authenticated users to the dashboard.
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            // Display an error if login fails.
            ModelState.AddModelError("", "Invalid email or password.");

            return View(model);
        }

        // Signs the current user out and returns them to the login page.
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}