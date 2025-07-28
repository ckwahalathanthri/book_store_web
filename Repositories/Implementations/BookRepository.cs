using BookStoreEcommerce.Data;
using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEcommerce.Repositories.Implementations
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(b => b.Category)
                .Where(b => b.CategoryId == categoryId && b.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
        {
            return await _dbSet
                .Include(b => b.Category)
                .Where(b => b.IsActive &&
                    (!string.IsNullOrEmpty(b.Title) && b.Title.Contains(searchTerm) ||
                     !string.IsNullOrEmpty(b.Author) && b.Author.Contains(searchTerm) ||
                     !string.IsNullOrEmpty(b.Description) && b.Description.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetFeaturedBooksAsync()
        {
            return await _dbSet
                .Include(b => b.Category)
                .Where(b => b.IsActive && b.StockQuantity > 0)
                .OrderByDescending(b => b.CreatedDate)
                .Take(8)
                .ToListAsync();
        }

        public async Task<Book?> GetBookWithDetailsAsync(int bookId)
        {
            return await _dbSet
                .Include(b => b.Category)
                .Include(b => b.Feedbacks)
                    .ThenInclude(f => f.Customer)
                        .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(b => b.BookId == bookId);
        }

        public async Task UpdateStockAsync(int bookId, int quantity)
        {
            var book = await GetByIdAsync(bookId);
            if (book != null)
            {
                book.StockQuantity = quantity;
                Update(book);
                await SaveChangesAsync();
            }
        }
    }
}
