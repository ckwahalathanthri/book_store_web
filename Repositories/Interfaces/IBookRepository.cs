using BookStoreEcommerce.Models.Entities;

namespace BookStoreEcommerce.Repositories.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId);
        Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm);
        Task<IEnumerable<Book>> GetFeaturedBooksAsync();
        Task<Book?> GetBookWithDetailsAsync(int bookId);
        Task UpdateStockAsync(int bookId, int quantity);
    }
}