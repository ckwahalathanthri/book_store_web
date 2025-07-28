using BookStoreEcommerce.Data;
using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEcommerce.Repositories.Implementations
{
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByBookIdAsync(int bookId)
        {
            return await _dbSet
                .Include(f => f.Customer)
                    .ThenInclude(c => c.User)
                .Where(f => f.BookId == bookId && f.IsApproved)
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Include(f => f.Book)
                .Where(f => f.CustomerId == customerId)
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetPendingFeedbacksAsync()
        {
            return await _dbSet
                .Include(f => f.Customer)
                    .ThenInclude(c => c.User)
                .Include(f => f.Book)
                .Where(f => !f.IsApproved)
                .OrderBy(f => f.CreatedDate)
                .ToListAsync();
        }
    }
}