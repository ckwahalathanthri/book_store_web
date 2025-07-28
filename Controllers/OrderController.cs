using BookStoreEcommerce.Repositories.Interfaces;
using BookStoreEcommerce.Services.Interfaces;
using BookStoreEcommerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreEcommerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserRepository _userRepository;

        public OrderController(IOrderService orderService, IUserRepository userRepository)
        {
            _orderService = orderService;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var user = SessionHelper.GetCurrentUser(HttpContext.Session);
            if (user == null || user.UserType != Models.Entities.UserType.Customer)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = await _userRepository.GetCustomerByUserIdAsync(user.UserId);
            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _orderService.GetOrdersByCustomerIdAsync(customer.CustomerId);
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = SessionHelper.GetCurrentUser(HttpContext.Session);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // Check if customer owns this order or if user is admin
            if (user.UserType == Models.Entities.UserType.Customer)
            {
                var customer = await _userRepository.GetCustomerByUserIdAsync(user.UserId);
                if (customer == null || order.CustomerId != customer.CustomerId)
                {
                    return Forbid();
                }
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(string shippingAddress, string paymentMethod)
        {
            var user = SessionHelper.GetCurrentUser(HttpContext.Session);
            if (user == null || user.UserType != Models.Entities.UserType.Customer)
            {
                return Json(new { success = false, message = "Please login to place an order." });
            }

            var customer = await _userRepository.GetCustomerByUserIdAsync(user.UserId);
            if (customer == null)
            {
                return Json(new { success = false, message = "Customer not found." });
            }

            var result = await _orderService.CreateOrderAsync(customer.CustomerId, shippingAddress, paymentMethod);

            if (result)
            {
                return Json(new { success = true, message = "Order placed successfully!" });
            }

            return Json(new { success = false, message = "Failed to place order." });
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var user = SessionHelper.GetCurrentUser(HttpContext.Session);
            if (user == null)
            {
                return Json(new { success = false, message = "Please login." });
            }

            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return Json(new { success = false, message = "Order not found." });
            }

            // Check ownership for customers
            if (user.UserType == Models.Entities.UserType.Customer)
            {
                var customer = await _userRepository.GetCustomerByUserIdAsync(user.UserId);
                if (customer == null || order.CustomerId != customer.CustomerId)
                {
                    return Json(new { success = false, message = "Unauthorized." });
                }
            }

            var result = await _orderService.CancelOrderAsync(id);

            if (result)
            {
                return Json(new { success = true, message = "Order cancelled successfully." });
            }

            return Json(new { success = false, message = "Failed to cancel order." });
        }
    }
}