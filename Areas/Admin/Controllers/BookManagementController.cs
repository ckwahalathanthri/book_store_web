using BookStoreEcommerce.Models.ViewModels;
using BookStoreEcommerce.Services.Interfaces;
using BookStoreEcommerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreEcommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookManagementController : Controller
    {
        private readonly IBookService _bookService;

        public BookManagementController(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return RedirectToAction("Login", "Admin");
            }

            var books = await _bookService.GetAllBooksAsync();
            return View(books);
        }

        public async Task<IActionResult> Create()
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return RedirectToAction("Login", "Admin");
            }

            ViewBag.Categories = await _bookService.GetCategoriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return RedirectToAction("Login", "Admin");
            }

            if (ModelState.IsValid)
            {
                var result = await _bookService.CreateBookAsync(model);
                if (result)
                {
                    TempData["Success"] = "Book created successfully!";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Failed to create book.");
            }

            ViewBag.Categories = await _bookService.GetCategoriesAsync();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return RedirectToAction("Login", "Admin");
            }

            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var model = new BookViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Description = book.Description,
                Price = book.Price,
                StockQuantity = book.StockQuantity,
                CategoryId = book.CategoryId,
                Publisher = book.Publisher,
                PublishedDate = book.PublishedDate,
                Pages = book.Pages,
                Language = book.Language,
                ImageUrl = book.ImageUrl
            };

            ViewBag.Categories = await _bookService.GetCategoriesAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookViewModel model)
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return RedirectToAction("Login", "Admin");
            }

            if (ModelState.IsValid)
            {
                var result = await _bookService.UpdateBookAsync(model);
                if (result)
                {
                    TempData["Success"] = "Book updated successfully!";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Failed to update book.");
            }

            ViewBag.Categories = await _bookService.GetCategoriesAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return RedirectToAction("Login", "Admin");
            }

            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, string deleteReason)
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            var result = await _bookService.DeleteBookAsync(id, deleteReason);

            if (result)
            {
                return Json(new { success = true, message = "Book deleted successfully!" });
            }

            return Json(new { success = false, message = "Failed to delete book." });
        }
    }
}
