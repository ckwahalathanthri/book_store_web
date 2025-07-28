using BookStoreEcommerce.Models.Entities;
using BookStoreEcommerce.Repositories.Interfaces;
using BookStoreEcommerce.Services.Interfaces;

namespace BookStoreEcommerce.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;
        private readonly IBookRepository _bookRepository;

        public OrderService(
            IOrderRepository orderRepository,
            ICartService cartService,
            IBookRepository bookRepository)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
            _bookRepository = bookRepository;
        }

        public async Task<bool> CreateOrderAsync(int customerId, string shippingAddress, string paymentMethod)
        {
            var cart = await _cartService.GetCartByCustomerIdAsync(customerId);
            if (cart == null || !cart.CartItems.Any())
                return false;

            // Check stock availability
            foreach (var item in cart.CartItems)
            {
                var book = await _bookRepository.GetByIdAsync(item.BookId);
                if (book == null || book.StockQuantity < item.Quantity)
                    return false;
            }

            var orderNumber = await _orderRepository.GenerateOrderNumberAsync();

            var order = new Order
            {
                CustomerId = customerId,
                OrderNumber = orderNumber,
                TotalAmount = cart.TotalAmount,
                ShippingAddress = shippingAddress,
                PaymentMethod = paymentMethod,
                OrderStatus = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            // Create order details
            foreach (var item in cart.CartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    TotalPrice = item.Price * item.Quantity
                };

                order.OrderDetails.Add(orderDetail);

                // Update book stock
                var book = await _bookRepository.GetByIdAsync(item.BookId);
                if (book != null)
                {
                    book.StockQuantity -= item.Quantity;
                    _bookRepository.Update(book);
                }
            }

            await _orderRepository.SaveChangesAsync();

            // Clear cart
            await _cartService.ClearCartAsync(customerId);

            return true;
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetOrderWithDetailsAsync(orderId);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;

            order.OrderStatus = status;

            if (status == OrderStatus.Shipped)
            {
                order.ShippedDate = DateTime.Now;
            }
            else if (status == OrderStatus.Delivered)
            {
                order.DeliveredDate = DateTime.Now;
                order.PaymentStatus = PaymentStatus.Paid;
            }

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            if (order == null || order.OrderStatus != OrderStatus.Pending)
                return false;

            order.OrderStatus = OrderStatus.Cancelled;

            // Restore book stock
            foreach (var detail in order.OrderDetails)
            {
                var book = await _bookRepository.GetByIdAsync(detail.BookId);
                if (book != null)
                {
                    book.StockQuantity += detail.Quantity;
                    _bookRepository.Update(book);
                }
            }

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetOrdersWithDetailsAsync();
        }
    }
}