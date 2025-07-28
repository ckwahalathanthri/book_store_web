using BookStoreEcommerce.Models.ViewModels;
using BookStoreEcommerce.Repositories.Interfaces;
using BookStoreEcommerce.Services.Interfaces;

namespace BookStoreEcommerce.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;

        public DashboardService(
            IBookRepository bookRepository,
            IUserRepository userRepository,
            IOrderRepository orderRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var allBooks = await _bookRepository.GetAllAsync();
            var allUsers = await _userRepository.GetAllAsync();
            var allOrders = await _orderRepository.GetAllAsync();

            var dashboard = new DashboardViewModel
            {
                TotalBooks = allBooks.Count(),
                TotalCustomers = allUsers.Count(u => u.UserType == Models.Entities.UserType.Customer),
                TotalOrders = allOrders.Count(),
                TotalRevenue = allOrders.Sum(o => o.TotalAmount),
                LowStockBooks = allBooks.Where(b => b.StockQuantity < 10).Count(),
                PendingOrders = allOrders.Count(o => o.OrderStatus == Models.Entities.OrderStatus.Pending),
                RecentOrders = allOrders.OrderByDescending(o => o.OrderDate).Take(5),
                TopSellingBooks = await GetTopSellingBooksAsync()
            };

            return dashboard;
        }

        private async Task<IEnumerable<object>> GetTopSellingBooksAsync()
        {
            var orders = await _orderRepository.GetOrdersWithDetailsAsync();

            var topBooks = orders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(od => new { od.BookId, od.Book.Title })
                .Select(g => new
                {
                    BookId = g.Key.BookId,
                    Title = g.Key.Title,
                    TotalSold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(5);

            return topBooks;
        }
    }
}