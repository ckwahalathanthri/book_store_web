using System.ComponentModel.DataAnnotations;

namespace BookStoreEcommerce.Models.Entities
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Phone]
        [StringLength(15)]
        public string? Phone { get; set; }

        [Required]
        public UserType UserType { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        // Navigation properties
        public virtual Customer? Customer { get; set; }
        public virtual Admin? Admin { get; set; }
    }

    public enum UserType
    {
        Admin,
        Customer
    }
}
