using BookStoreEcommerce.Data;
using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEcommerce.Repositories.Implementations
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Cart?> GetCartWithItemsAsync(int customerId)
        {
            return await _dbSet
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Book)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task<CartItem?> GetCartItemAsync(int cartItemId)
        {
            return await _context.CartItems
                .Include(ci => ci.Cart)
                .Include(ci => ci.Book)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
        }

        public void RemoveCartItem(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
        }
    }
}
