using BookStoreEcommerce.Models.ViewModels;

namespace BookStoreEcommerce.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync();
    }
}