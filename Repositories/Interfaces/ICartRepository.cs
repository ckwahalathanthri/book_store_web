using BookStoreEcommerce.Models.Entities;

namespace BookStoreEcommerce.Repositories.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetCartWithItemsAsync(int customerId);
        Task<CartItem?> GetCartItemAsync(int cartItemId);
        void RemoveCartItem(CartItem cartItem);
    }
}