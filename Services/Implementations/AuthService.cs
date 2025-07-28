using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Models.ViewModels;
using BookStoreEcommerce.Repositories.Interfaces;
using BookStoreEcommerce.Services.Interfaces;

namespace BookStoreEcommerce.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user != null && VerifyPassword(password, user.Password))
            {
                return user;
            }

            return null;
        }

        public async Task<bool> RegisterCustomerAsync(RegisterViewModel model)
        {
            if (await EmailExistsAsync(model.Email))
            {
                return false;
            }

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = HashPassword(model.Password),
                Phone = model.Phone,
                UserType = UserType.Customer
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var customer = new Customer
            {
                UserId = user.UserId,
                Address = model.Address,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender
            };

            await _userRepository.AddCustomerAsync(customer);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}