using System.ComponentModel.DataAnnotations;

namespace BookStoreEcommerce.Models.ViewModels
{
    public class BookViewModel
    {
        public int BookId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;

        [StringLength(20)]
        public string? ISBN { get; set; }

        public string? Description { get; set; }

        [Required]
        [Range(0.01, 9999.99)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [StringLength(100)]
        public string? Publisher { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PublishedDate { get; set; }

        public int? Pages { get; set; }

        [StringLength(50)]
        public string Language { get; set; } = "English";

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        public string? CategoryName { get; set; }
    }
}
