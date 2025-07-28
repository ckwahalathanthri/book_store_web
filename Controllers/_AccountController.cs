using BookStoreEcommerce.Models.ViewModels;
using BookStoreEcommerce.Services.Interfaces;
using BookStoreEcommerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreEcommerce.Controllers
{
    public class _AccountController : Controller
    {
        private readonly IAuthService _authService;

        public _AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await _authService.AuthenticateAsync(model.Email, model.Password);

                if (user != null)
                {
                    SessionHelper.SetUser(HttpContext.Session, user);

                    if (user.UserType == Models.Entities.UserType.Admin)
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid email or password.");
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterCustomerAsync(model);

                if (result)
                {
                    TempData["Success"] = "Registration successful! Please login.";
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError("", "Email already exists.");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            SessionHelper.ClearSession(HttpContext.Session);
            return RedirectToAction("Index", "Home");
        }
    }
}