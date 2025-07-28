using BookStoreEcommerce.Models.Entities;

namespace BookStoreEcommerce.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetUserWithDetailsAsync(int userId);
        Task<Customer?> GetCustomerByUserIdAsync(int userId);
        Task<Admin?> GetAdminByUserIdAsync(int userId);
        Task AddCustomerAsync(Customer customer);
    }
}
