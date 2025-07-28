using BookStoreEcommerce.Models.Entities;

namespace BookStoreEcommerce.Models.ViewModels
{
    public class CustomerManagementViewModel
    {
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime? LastOrderDate { get; set; }
        public bool IsActive { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

    public class CustomerDetailViewModel : CustomerManagementViewModel
    {
        public DateTime LastLoginDate { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public decimal AverageOrderValue { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
    }

    public class CustomerStatisticsViewModel
    {
        public int TotalCustomers { get; set; }
        public int InactiveCustomers { get; set; }
        public int NewCustomersThisMonth { get; set; }
        public int NewCustomersThisWeek { get; set; }
        public int CustomersWithOrders { get; set; }
        public int CustomersWithoutOrders { get; set; }
        public double AverageOrdersPerCustomer { get; set; }
        public List<CustomerSpendingViewModel> TopSpendingCustomers { get; set; } = new List<CustomerSpendingViewModel>();
    }

    public class CustomerSpendingViewModel
    {
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime LastOrderDate { get; set; }
    }
}