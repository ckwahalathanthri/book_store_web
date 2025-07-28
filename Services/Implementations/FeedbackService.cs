using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Models.ViewModels;
using BookStoreEcommerce.Repositories.Interfaces;
using BookStoreEcommerce.Services.Interfaces;

namespace BookStoreEcommerce.Services.Implementations
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<bool> CreateFeedbackAsync(FeedbackViewModel model)
        {
            var feedback = new Feedback
            {
                CustomerId = model.CustomerId,
                BookId = model.BookId,
                Rating = model.Rating,
                Comment = model.Comment
            };

            await _feedbackRepository.AddAsync(feedback);
            await _feedbackRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByBookIdAsync(int bookId)
        {
            return await _feedbackRepository.GetFeedbacksByBookIdAsync(bookId);
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByCustomerIdAsync(int customerId)
        {
            return await _feedbackRepository.GetFeedbacksByCustomerIdAsync(customerId);
        }

        public async Task<bool> ApproveFeedbackAsync(int feedbackId)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackId);
            if (feedback == null) return false;

            feedback.IsApproved = true;
            _feedbackRepository.Update(feedback);
            await _feedbackRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFeedbackAsync(int feedbackId)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackId);
            if (feedback == null) return false;

            _feedbackRepository.Delete(feedback);
            await _feedbackRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Feedback>> GetPendingFeedbacksAsync()
        {
            return await _feedbackRepository.GetPendingFeedbacksAsync();
        }
    }
}