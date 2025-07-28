using BookStoreEcommerce.Models.Entities;

namespace BookStoreEcommerce.Repositories.Interfaces
{
    public interface IFeedbackRepository : IRepository<Feedback>
    {
        Task<IEnumerable<Feedback>> GetFeedbacksByBookIdAsync(int bookId);
        Task<IEnumerable<Feedback>> GetFeedbacksByCustomerIdAsync(int customerId);
        Task<IEnumerable<Feedback>> GetPendingFeedbacksAsync();
    }
}