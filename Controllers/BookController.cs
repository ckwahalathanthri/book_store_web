using BookStoreEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreEcommerce.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IActionResult> Index(int? categoryId, string? search)
        {
            IEnumerable<BookStoreEcommerce.Models.Entities.Book> books;

            if (!string.IsNullOrEmpty(search))
            {
                books = await _bookService.SearchBooksAsync(search);
                ViewBag.SearchTerm = search;
            }
            else if (categoryId.HasValue)
            {
                books = await _bookService.GetBooksByCategoryAsync(categoryId.Value);
                ViewBag.CategoryId = categoryId;
            }
            else
            {
                books = await _bookService.GetAllBooksAsync();
            }

            ViewBag.Categories = await _bookService.GetCategoriesAsync();
            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index");
            }

            var books = await _bookService.SearchBooksAsync(searchTerm);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.Categories = await _bookService.GetCategoriesAsync();

            return View("Index", books);
        }
    }
}