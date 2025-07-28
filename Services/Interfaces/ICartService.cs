using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Models.ViewModels;

namespace BookStoreEcommerce.Services.Interfaces
{
    public interface ICartService
    {
        Task<Cart?> GetCartByCustomerIdAsync(int customerId);
        Task<bool> AddToCartAsync(int customerId, int bookId, int quantity = 1);
        Task<bool> UpdateCartItemAsync(int cartItemId, int quantity);
        Task<bool> RemoveFromCartAsync(int cartItemId);
        Task<bool> ClearCartAsync(int customerId);
        Task<CartViewModel> GetCartViewModelAsync(int customerId);
    }
}