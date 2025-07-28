using BookStoreEcommerce.Models.ViewModels;
using BookStoreEcommerce.Services.Interfaces;
using BookStoreEcommerce.Repositories.Interfaces;
using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreEcommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IOrderService _orderService;

        public AccountController(
            IAuthService authService,
            IUserRepository userRepository,
            IOrderService orderService)
        {
            _authService = authService;
            _userRepository = userRepository;
            _orderService = orderService;
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

        public async Task<IActionResult> Profile()
        {
            var user = SessionHelper.GetCurrentUser(HttpContext.Session);
            if (user == null || user.UserType != UserType.Customer)
            {
                return RedirectToAction("Login");
            }

            // Get customer details
            var customer = await _userRepository.GetCustomerByUserIdAsync(user.UserId);
            if (customer == null)
            {
                TempData["Error"] = "Customer profile not found.";
                return RedirectToAction("Login");
            }

            // Get order statistics
            var orders = await _orderService.GetOrdersByCustomerIdAsync(customer.CustomerId);

            // Create view model with existing data
            var model = new RegisterViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Address = customer.Address,
                DateOfBirth = customer.DateOfBirth,
                Gender = customer.Gender
            };

            // Set ViewBag data for statistics
            ViewBag.TotalOrders = orders.Count();
            ViewBag.TotalSpent = orders.Sum(o => o.TotalAmount);
            ViewBag.MemberSince = user.CreatedDate;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(RegisterViewModel model, string? currentPassword = null)
        {
            var user = SessionHelper.GetCurrentUser(HttpContext.Session);
            if (user == null || user.UserType != UserType.Customer)
            {
                return RedirectToAction("Login");
            }

            // Remove password validation if not changing password
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

            // Custom validation for password change
            if (!string.IsNullOrEmpty(model.Password))
            {
                if (string.IsNullOrEmpty(currentPassword))
                {
                    ModelState.AddModelError("", "Current password is required to change password.");
                }
                else
                {
                    // Verify current password
                    var currentUser = await _userRepository.GetByIdAsync(user.UserId);
                    if (currentUser == null || !_authService.VerifyPassword(currentPassword, currentUser.Password))
                    {
                        ModelState.AddModelError("", "Current password is incorrect.");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the current user and customer from database
                    var dbUser = await _userRepository.GetByIdAsync(user.UserId);
                    var customer = await _userRepository.GetCustomerByUserIdAsync(user.UserId);

                    if (dbUser == null || customer == null)
                    {
                        TempData["Error"] = "User not found.";
                        return View(model);
                    }

                    // Update user information
                    dbUser.FirstName = model.FirstName;
                    dbUser.LastName = model.LastName;
                    dbUser.Phone = model.Phone;
                    dbUser.UpdatedDate = DateTime.Now;

                    // Update password if provided
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        dbUser.Password = _authService.HashPassword(model.Password);
                    }

                    // Update customer information
                    customer.Address = model.Address;
                    customer.DateOfBirth = model.DateOfBirth;
                    customer.Gender = model.Gender;

                    // Save changes
                    _userRepository.Update(dbUser);
                    await _userRepository.SaveChangesAsync();

                    // Update session with new user data
                    SessionHelper.SetUser(HttpContext.Session, dbUser);

                    TempData["Success"] = "Profile updated successfully!";
                    return RedirectToAction("Profile");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "An error occurred while updating your profile. Please try again.";
                    
                    // Log the exception (you should implement proper logging)
                    // _logger.LogError(ex, "Error updating user profile for UserId: {UserId}", user.UserId);
                }
            }

            // If we got this far, something failed, redisplay form
            // Reload statistics for ViewBag
            var customer_reload = await _userRepository.GetCustomerByUserIdAsync(user.UserId);
            if (customer_reload != null)
            {
                var orders = await _orderService.GetOrdersByCustomerIdAsync(customer_reload.CustomerId);
                ViewBag.TotalOrders = orders.Count();
                ViewBag.TotalSpent = orders.Sum(o => o.TotalAmount);
                ViewBag.MemberSince = user.CreatedDate;
            }

            return View("Profile", model);
        }

        public IActionResult Logout()
        {
            SessionHelper.ClearSession(HttpContext.Session);
            return RedirectToAction("Index", "Home");
        }
    }
}