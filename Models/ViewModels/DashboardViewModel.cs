using BookStoreEcommerce.Models.Entities;

namespace BookStoreEcommerce.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalBooks { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int LowStockBooks { get; set; }
        public int PendingOrders { get; set; }
        public IEnumerable<Order> RecentOrders { get; set; } = new List<Order>();
        public IEnumerable<object> TopSellingBooks { get; set; } = new List<object>();
    }
}
