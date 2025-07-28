using BookStoreEcommerce.Repositories.Interfaces;
using BookStoreEcommerce.Services.Interfaces;
using BookStoreEcommerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreEcommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IUserRepository _userRepository;

        public CartController(ICartService cartService, IUserRepository userRepository)
        {
            _cartService = cartService;
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

            var cartViewModel = await _cartService.GetCartViewModelAsync(customer.CustomerId);
            return View(cartViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int bookId, int quantity = 1)
        {
            var user = SessionHelper.GetCurrentUser(HttpContext.Session);
            if (user == null || user.UserType != Models.Entities.UserType.Customer)
            {
                return Json(new { success = false, message = "Please login to add items to cart." });
            }

            var customer = await _userRepository.GetCustomerByUserIdAsync(user.UserId);
            if (customer == null)
            {
                return Json(new { success = false, message = "Customer not found." });
            }

            var result = await _cartService.AddToCartAsync(customer.CustomerId, bookId, quantity);

            if (result)
            {
                return Json(new { success = true, message = "Item added to cart successfully." });
            }

            return Json(new { success = false, message = "Failed to add item to cart." });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var result = await _cartService.UpdateCartItemAsync(cartItemId, quantity);

            if (result)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var result = await _cartService.RemoveFromCartAsync(cartItemId);

            if (result)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
    }
}