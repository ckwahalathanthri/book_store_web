using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Models.ViewModels;
using BookStoreEcommerce.Repositories.Interfaces;
using BookStoreEcommerce.Services.Interfaces;
using BookStoreEcommerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreEcommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerManagementController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderService _orderService;

        public CustomerManagementController(IUserRepository userRepository, IOrderService orderService)
        {
            _userRepository = userRepository;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return RedirectToAction("Login", "Admin");
            }

            var allUsers = await _userRepository.GetAllAsync();
            var customers = allUsers
                .Where(u => u.UserType == UserType.Customer && u.IsActive)
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                customers = customers.Where(c =>
                    c.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    c.LastName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
                ViewBag.SearchTerm = search;
            }

            // Apply pagination
            var totalCustomers = customers.Count();
            var paginatedCustomers = customers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(c => c.CreatedDate)
                .ToList();

            // Create customer view models with additional info
            var customerViewModels = new List<CustomerManagementViewModel>();
            foreach (var user in paginatedCustomers)
            {
                var customer = await _userRepository.GetCustomerByUserIdAsync(user.UserId);
                var orders = await _orderService.GetOrdersByCustomerIdAsync(customer?.CustomerId ?? 0);

                customerViewModels.Add(new CustomerManagementViewModel
                {
                    UserId = user.UserId,
                    CustomerId = customer?.CustomerId ?? 0,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Address = customer?.Address,
                    DateOfBirth = customer?.DateOfBirth,
                    Gender = customer?.Gender,
                    RegistrationDate = user.CreatedDate,
                    TotalOrders = orders.Count(),
                    TotalSpent = orders.Sum(o => o.TotalAmount),
                    LastOrderDate = orders.OrderByDescending(o => o.OrderDate).FirstOrDefault()?.OrderDate,
                    IsActive = user.IsActive
                });
            }

            // Pagination info
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCustomers / pageSize);
            ViewBag.TotalCustomers = totalCustomers;

            return View(customerViewModels);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return RedirectToAction("Login", "Admin");
            }

            var user = await _userRepository.GetUserWithDetailsAsync(id);
            if (user == null || user.UserType != UserType.Customer)
            {
                return NotFound();
            }

            var customer = await _userRepository.GetCustomerByUserIdAsync(user.UserId);
            var orders = customer != null ? await _orderService.GetOrdersByCustomerIdAsync(customer.CustomerId) : new List<Order>();

            var viewModel = new CustomerDetailViewModel
            {
                UserId = user.UserId,
                CustomerId = customer?.CustomerId ?? 0,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Address = customer?.Address,
                DateOfBirth = customer?.DateOfBirth,
                Gender = customer?.Gender,
                RegistrationDate = user.CreatedDate,
                LastLoginDate = user.UpdatedDate,
                IsActive = user.IsActive,
                Orders = orders.OrderByDescending(o => o.OrderDate).ToList(),
                TotalOrders = orders.Count(),
                TotalSpent = orders.Sum(o => o.TotalAmount),
                AverageOrderValue = orders.Any() ? orders.Average(o => o.TotalAmount) : 0,
                LastOrderDate = orders.OrderByDescending(o => o.OrderDate).FirstOrDefault()?.OrderDate,
                PendingOrders = orders.Count(o => o.OrderStatus == OrderStatus.Pending),
                CompletedOrders = orders.Count(o => o.OrderStatus == OrderStatus.Delivered)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null || user.UserType != UserType.Customer)
            {
                return Json(new { success = false, message = "Customer not found." });
            }

            user.IsActive = !user.IsActive;
            user.UpdatedDate = DateTime.Now;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            var status = user.IsActive ? "activated" : "deactivated";
            return Json(new { success = true, message = $"Customer {status} successfully.", isActive = user.IsActive });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null || user.UserType != UserType.Customer)
            {
                return Json(new { success = false, message = "Customer not found." });
            }

            // Check if customer has any orders
            var customer = await _userRepository.GetCustomerByUserIdAsync(user.UserId);
            if (customer != null)
            {
                var orders = await _orderService.GetOrdersByCustomerIdAsync(customer.CustomerId);
                if (orders.Any())
                {
                    return Json(new { success = false, message = "Cannot delete customer with existing orders. Deactivate instead." });
                }
            }

            // Soft delete by setting IsActive to false
            user.IsActive = false;
            user.UpdatedDate = DateTime.Now;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return Json(new { success = true, message = "Customer deleted successfully." });
        }

        public async Task<IActionResult> Export(string format = "csv")
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return RedirectToAction("Login", "Admin");
            }

            var allUsers = await _userRepository.GetAllAsync();
            var customers = allUsers.Where(u => u.UserType == UserType.Customer && u.IsActive).ToList();

            var customerData = new List<object>();
            foreach (var user in customers)
            {
                var customer = await _userRepository.GetCustomerByUserIdAsync(user.UserId);
                var orders = customer != null ? await _orderService.GetOrdersByCustomerIdAsync(customer.CustomerId) : new List<Order>();

                customerData.Add(new
                {
                    Name = $"{user.FirstName} {user.LastName}",
                    Email = user.Email,
                    Phone = user.Phone ?? "N/A",
                    Address = customer?.Address ?? "N/A",
                    RegistrationDate = user.CreatedDate.ToString("yyyy-MM-dd"),
                    TotalOrders = orders.Count(),
                    TotalSpent = orders.Sum(o => o.TotalAmount),
                    LastOrderDate = orders.OrderByDescending(o => o.OrderDate).FirstOrDefault()?.OrderDate.ToString("yyyy-MM-dd") ?? "N/A"
                });
            }

            if (format.ToLower() == "csv")
            {
                var csv = "Name,Email,Phone,Address,Registration Date,Total Orders,Total Spent,Last Order Date\n";
                foreach (var item in customerData)
                {
                    var props = item.GetType().GetProperties();
                    var values = props.Select(p => p.GetValue(item)?.ToString() ?? "").ToArray();
                    csv += string.Join(",", values) + "\n";
                }

                var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
                return File(bytes, "text/csv", $"customers_{DateTime.Now:yyyyMMdd}.csv");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Statistics()
        {
            if (!SessionHelper.IsAdmin(HttpContext.Session))
            {
                return RedirectToAction("Login", "Admin");
            }

            var allUsers = await _userRepository.GetAllAsync();
            var customers = allUsers.Where(u => u.UserType == UserType.Customer).ToList();

            var stats = new CustomerStatisticsViewModel
            {
                TotalCustomers = customers.Count(c => c.IsActive),
                InactiveCustomers = customers.Count(c => !c.IsActive),
                NewCustomersThisMonth = customers.Count(c => c.IsActive && c.CreatedDate >= DateTime.Now.AddMonths(-1)),
                NewCustomersThisWeek = customers.Count(c => c.IsActive && c.CreatedDate >= DateTime.Now.AddDays(-7)),
                CustomersWithOrders = 0,
                CustomersWithoutOrders = 0,
                AverageOrdersPerCustomer = 0,
                TopSpendingCustomers = new List<CustomerSpendingViewModel>()
            };

            var customersWithOrderData = new List<CustomerSpendingViewModel>();

            foreach (var user in customers.Where(c => c.IsActive))
            {
                var customer = await _userRepository.GetCustomerByUserIdAsync(user.UserId);
                if (customer != null)
                {
                    var orders = await _orderService.GetOrdersByCustomerIdAsync(customer.CustomerId);
                    var totalSpent = orders.Sum(o => o.TotalAmount);

                    if (orders.Any())
                    {
                        stats.CustomersWithOrders++;
                        customersWithOrderData.Add(new CustomerSpendingViewModel
                        {
                            CustomerName = user.FullName,
                            Email = user.Email,
                            TotalOrders = orders.Count(),
                            TotalSpent = totalSpent,
                            LastOrderDate = orders.OrderByDescending(o => o.OrderDate).First().OrderDate
                        });
                    }
                    else
                    {
                        stats.CustomersWithoutOrders++;
                    }
                }
            }

            stats.AverageOrdersPerCustomer = stats.CustomersWithOrders > 0
                ? customersWithOrderData.Average(c => c.TotalOrders)
                : 0;

            stats.TopSpendingCustomers = customersWithOrderData
                .OrderByDescending(c => c.TotalSpent)
                .Take(10)
                .ToList();

            return View(stats);
        }
    }
}