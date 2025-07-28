using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Models.ViewModels;

namespace BookStoreEcommerce.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<bool> CreateFeedbackAsync(FeedbackViewModel model);
        Task<IEnumerable<Feedback>> GetFeedbacksByBookIdAsync(int bookId);
        Task<IEnumerable<Feedback>> GetFeedbacksByCustomerIdAsync(int customerId);
        Task<bool> ApproveFeedbackAsync(int feedbackId);
        Task<bool> DeleteFeedbackAsync(int feedbackId);
        Task<IEnumerable<Feedback>> GetPendingFeedbacksAsync();
    }
}