using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Models.ViewModels;

namespace BookStoreEcommerce.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId);
        Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm);
        Task<IEnumerable<Book>> GetFeaturedBooksAsync();
        Task<bool> CreateBookAsync(BookViewModel model);
        Task<bool> UpdateBookAsync(BookViewModel model);
        Task<bool> DeleteBookAsync(int id, string deleteReason);
        Task<IEnumerable<Category>> GetCategoriesAsync();
    }
}