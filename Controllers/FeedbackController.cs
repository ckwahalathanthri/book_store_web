using BookStoreEcommerce.Models.ViewModels;
using BookStoreEcommerce.Repositories.Interfaces;
using BookStoreEcommerce.Services.Interfaces;
using BookStoreEcommerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreEcommerce.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IUserRepository _userRepository;
        private readonly IBookService _bookService;

        public FeedbackController(
            IFeedbackService feedbackService,
            IUserRepository userRepository,
            IBookService bookService)
        {
            _feedbackService = feedbackService;
            _userRepository = userRepository;
            _bookService = bookService;
        }

        public async Task<IActionResult> Create(int bookId)
        {
            var user = SessionHelper.GetCurrentUser(HttpContext.Session);
            if (user == null || user.UserType != Models.Entities.UserType.Customer)
            {
                return RedirectToAction("Login", "Account");
            }

            var book = await _bookService.GetBookByIdAsync(bookId);
            if (book == null)
            {
                return NotFound();
            }

            var customer = await _userRepository.GetCustomerByUserIdAsync(user.UserId);
            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new FeedbackViewModel
            {
                BookId = bookId,
                CustomerId = customer.CustomerId,
                BookTitle = book.Title
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FeedbackViewModel model)
        {
            var user = SessionHelper.GetCurrentUser(HttpContext.Session);
            if (user == null || user.UserType != Models.Entities.UserType.Customer)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var result = await _feedbackService.CreateFeedbackAsync(model);
                if (result)
                {
                    TempData["Success"] = "Thank you for your feedback! It will be reviewed before being published.";
                    return RedirectToAction("Details", "Book", new { id = model.BookId });
                }

                ModelState.AddModelError("", "Failed to submit feedback.");
            }

            var book = await _bookService.GetBookByIdAsync(model.BookId);
            model.BookTitle = book?.Title;

            return View(model);
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

            var feedbacks = await _feedbackService.GetFeedbacksByCustomerIdAsync(customer.CustomerId);
            return View(feedbacks);
        }
    }
}