using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreEcommerce.Models.Entities
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        // Calculated properties
        [NotMapped]
        public decimal TotalAmount => CartItems.Sum(item => item.Price * item.Quantity);

        [NotMapped]
        public int TotalItems => CartItems.Sum(item => item.Quantity);
    }
}