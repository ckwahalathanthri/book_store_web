using BookStoreEcommerce.Models.Entities;

namespace BookStoreEcommerce.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersWithDetailsAsync();
        Task<string> GenerateOrderNumberAsync();
    }
}