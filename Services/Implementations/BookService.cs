using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Models.ViewModels;
using BookStoreEcommerce.Repositories.Interfaces;
using BookStoreEcommerce.Services.Interfaces;

namespace BookStoreEcommerce.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRepository<Category> _categoryRepository;

        public BookService(IBookRepository bookRepository, IRepository<Category> categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetBookWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _bookRepository.GetBooksByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
        {
            return await _bookRepository.SearchBooksAsync(searchTerm);
        }

        public async Task<IEnumerable<Book>> GetFeaturedBooksAsync()
        {
            return await _bookRepository.GetFeaturedBooksAsync();
        }

        public async Task<bool> CreateBookAsync(BookViewModel model)
        {
            var book = new Book
            {
                Title = model.Title,
                Author = model.Author,
                ISBN = model.ISBN,
                Description = model.Description,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                CategoryId = model.CategoryId,
                Publisher = model.Publisher,
                PublishedDate = model.PublishedDate,
                Pages = model.Pages,
                Language = model.Language,
                ImageUrl = model.ImageUrl
            };

            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateBookAsync(BookViewModel model)
        {
            var book = await _bookRepository.GetByIdAsync(model.BookId);
            if (book == null) return false;

            book.Title = model.Title;
            book.Author = model.Author;
            book.ISBN = model.ISBN;
            book.Description = model.Description;
            book.Price = model.Price;
            book.StockQuantity = model.StockQuantity;
            book.CategoryId = model.CategoryId;
            book.Publisher = model.Publisher;
            book.PublishedDate = model.PublishedDate;
            book.Pages = model.Pages;
            book.Language = model.Language;
            book.ImageUrl = model.ImageUrl;
            book.UpdatedDate = DateTime.Now;

            _bookRepository.Update(book);
            await _bookRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBookAsync(int id, string deleteReason)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null) return false;

            book.IsActive = false;
            _bookRepository.Update(book);
            await _bookRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }
    }
}
