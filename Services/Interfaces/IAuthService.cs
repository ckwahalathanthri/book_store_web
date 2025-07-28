using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Models.ViewModels;

namespace BookStoreEcommerce.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(string email, string password);
        Task<bool> RegisterCustomerAsync(RegisterViewModel model);
        Task<bool> EmailExistsAsync(string email);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}