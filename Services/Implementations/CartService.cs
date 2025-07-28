using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Models.ViewModels;
using BookStoreEcommerce.Repositories.Interfaces;
using BookStoreEcommerce.Services.Interfaces;

namespace BookStoreEcommerce.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IBookRepository _bookRepository;

        public CartService(ICartRepository cartRepository, IBookRepository bookRepository)
        {
            _cartRepository = cartRepository;
            _bookRepository = bookRepository;
        }

        public async Task<Cart?> GetCartByCustomerIdAsync(int customerId)
        {
            return await _cartRepository.GetCartWithItemsAsync(customerId);
        }

        public async Task<bool> AddToCartAsync(int customerId, int bookId, int quantity = 1)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null || book.StockQuantity < quantity)
                return false;

            var cart = await _cartRepository.GetCartWithItemsAsync(customerId);
            if (cart == null)
            {
                cart = new Cart { CustomerId = customerId };
                await _cartRepository.AddAsync(cart);
                await _cartRepository.SaveChangesAsync();
            }

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.BookId == bookId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                if (existingItem.Quantity > book.StockQuantity)
                    existingItem.Quantity = book.StockQuantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    BookId = bookId,
                    Quantity = quantity,
                    Price = book.Price
                };
                cart.CartItems.Add(cartItem);
            }

            cart.UpdatedDate = DateTime.Now;
            await _cartRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCartItemAsync(int cartItemId, int quantity)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(cartItemId);
            if (cartItem == null) return false;

            if (quantity <= 0)
            {
                return await RemoveFromCartAsync(cartItemId);
            }

            cartItem.Quantity = quantity;
            cartItem.Cart.UpdatedDate = DateTime.Now;
            await _cartRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromCartAsync(int cartItemId)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(cartItemId);
            if (cartItem == null) return false;

            _cartRepository.RemoveCartItem(cartItem);
            await _cartRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(int customerId)
        {
            var cart = await _cartRepository.GetCartWithItemsAsync(customerId);
            if (cart == null) return false;

            foreach (var item in cart.CartItems.ToList())
            {
                _cartRepository.RemoveCartItem(item);
            }

            await _cartRepository.SaveChangesAsync();
            return true;
        }

        public async Task<CartViewModel> GetCartViewModelAsync(int customerId)
        {
            var cart = await _cartRepository.GetCartWithItemsAsync(customerId);

            var viewModel = new CartViewModel
            {
                CartId = cart?.CartId ?? 0,
                Items = new List<CartItemViewModel>(),
                TotalAmount = 0,
                TotalItems = 0
            };

            if (cart != null)
            {
                viewModel.Items = cart.CartItems.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.CartItemId,
                    BookId = ci.BookId,
                    BookTitle = ci.Book.Title,
                    BookAuthor = ci.Book.Author,
                    BookImageUrl = ci.Book.ImageUrl,
                    Price = ci.Price,
                    Quantity = ci.Quantity,
                    TotalPrice = ci.TotalPrice
                }).ToList();

                viewModel.TotalAmount = cart.TotalAmount;
                viewModel.TotalItems = cart.TotalItems;
            }

            return viewModel;
        }
    }
}