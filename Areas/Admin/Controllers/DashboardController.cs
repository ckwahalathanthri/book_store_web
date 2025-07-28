using BookStoreEcommerce.Services.Interfaces;
using BookStoreEcommerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreEcommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }

            var dashboardData = await _dashboardService.GetDashboardDataAsync();
            return View(dashboardData);
        }
    }
}