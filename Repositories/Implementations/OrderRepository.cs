using BookStoreEcommerce.Data;
using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEcommerce.Repositories.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Book)
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _dbSet
                .Include(o => o.Customer)
                    .ThenInclude(c => c.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Book)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersWithDetailsAsync()
        {
            return await _dbSet
                .Include(o => o.Customer)
                    .ThenInclude(c => c.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Book)
                .ToListAsync();
        }

        public async Task<string> GenerateOrderNumberAsync()
        {
            var lastOrder = await _dbSet
                .OrderByDescending(o => o.OrderId)
                .FirstOrDefaultAsync();

            int nextOrderNumber = 1;
            if (lastOrder != null)
            {
                var lastNumber = lastOrder.OrderNumber.Substring(3); // Remove "ORD" prefix
                if (int.TryParse(lastNumber, out int number))
                {
                    nextOrderNumber = number + 1;
                }
            }

            return $"ORD{nextOrderNumber:D6}"; // Format: ORD000001
        }
    }
}
