using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreEcommerce.Models.Entities
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        public int UserId { get; set; }

        [StringLength(50)]
        public string? Department { get; set; }

        public int AccessLevel { get; set; } = 1;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}