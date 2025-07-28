using BookStoreEcommerce.Models.ViewModels;
using BookStoreEcommerce.Services.Interfaces;
using BookStoreEcommerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreEcommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly IAuthService _authService;

        public AdminController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            if (SessionHelper.IsAdmin(HttpContext.Session))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _authService.AuthenticateAsync(model.Email, model.Password);

                if (user != null && user.UserType == Models.Entities.UserType.Admin)
                {
                    SessionHelper.SetUser(HttpContext.Session, user);
                    return RedirectToAction("Index", "Dashboard");
                }

                ModelState.AddModelError("", "Invalid admin credentials.");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            SessionHelper.ClearSession(HttpContext.Session);
            return RedirectToAction("Login");
        }
    }
}