using System.ComponentModel.DataAnnotations;

namespace BookStoreEcommerce.Models.Entities
{
    public class Category : BaseEntity
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}