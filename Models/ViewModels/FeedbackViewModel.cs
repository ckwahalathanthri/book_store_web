using System.ComponentModel.DataAnnotations;

namespace BookStoreEcommerce.Models.ViewModels
{
    public class FeedbackViewModel
    {
        public int FeedbackId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters")]
        public string? Comment { get; set; }

        public string? BookTitle { get; set; }
        public string? CustomerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsApproved { get; set; }
    }
}
