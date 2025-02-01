using ECommerceApp.Models;
using ECommerceApp.Repositories;

namespace ECommerceApp.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;

        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void AddOrder(Order order)
        {
            _orderRepository.AddOrder(order);
        }

        public Order GetLastOrderByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            // Kullanıcının en son oluşturduğu siparişi getir
            var order = _orderRepository.GetOrdersByUserId(userId)
                                         .OrderByDescending(o => o.OrderDate)
                                         .FirstOrDefault();

            if (order == null)
            {
                throw new Exception("No orders found for this user.");
            }

            return order;
        }
    }
}
