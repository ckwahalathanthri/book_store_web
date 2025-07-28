using BookStoreEcommerce.Models.Entities;

namespace BookStoreEcommerce.Services.Interfaces
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync(int customerId, string shippingAddress, string paymentMethod);
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<bool> CancelOrderAsync(int orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
    }
}