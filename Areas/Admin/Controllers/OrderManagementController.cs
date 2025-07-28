using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Services.Interfaces;
using BookStoreEcommerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreEcommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderManagementController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderManagementController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return RedirectToAction("Login", "Admin");
            }

            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return RedirectToAction("Login", "Admin");
            }

            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, OrderStatus status)
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            var result = await _orderService.UpdateOrderStatusAsync(orderId, status);

            if (result)
            {
                return Json(new { success = true, message = "Order status updated successfully!" });
            }

            return Json(new { success = false, message = "Failed to update order status." });
        }
    }
}
