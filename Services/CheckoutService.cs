using ECommerceApp.Models;
using ECommerceApp.Repositories;
using ECommerceApp.Services;

namespace ECommerceApp.Services
{
    public class CheckoutService
    {
        private readonly AddressRepository _addressRepository;
        private readonly ShippingService _shippingService;
        private readonly PaymentRepository _paymentRepository;
        private readonly OrderRepository _orderRepository;
        private readonly ProductRepository _productRepository;
        private readonly CartService _cartService;
        private readonly AddressService _addressService;

        public CheckoutService(
            AddressService addressService,
            AddressRepository addressRepository,
            ShippingService shippingService,
            PaymentRepository paymentRepository,
            OrderRepository orderRepository,
            ProductRepository productRepository,
            CartService cartService)
        {
            _addressRepository = addressRepository;
            _shippingService = shippingService;
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _cartService = cartService;
            _addressService = addressService;
        }

        public void ProcessOrder(string userId, CheckoutViewModel model)
        {
            // Adres ekle
            var address = new Address
            {
                UserId = userId,
                FullName = model.FullName,
                Email = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                Street = model.Address,
                City = model.City,
                PostalCode = model.PostalCode,
                Country = model.Country
            };
            _addressService.AddAddress(address);

            // Kargo yöntemi ekle
            var shipping = new Shipping
            {
                UserId = userId,
                Method = model.ShippingMethod,
                Cost = model.ShippingCost,
                EstimatedDelivery = DateTime.Now.AddDays(5) // Örnek tahmini teslimat tarihi
            };
            _shippingService.AddShipping(shipping);

            // Kullanıcının sepetini al
            var cart = _cartService.GetCartByUserIdWithDetails(userId);
            if (cart == null || !cart.Items.Any())
            {
                throw new Exception("Your cart is empty!");
            }

            // Sipariş oluştur
            var order = new Order
            {
                UserId = userId,

                Items = cart.Items,
                TotalAmount = cart.Items.Sum(i => i.ProductPrice * i.Quantity) + shipping.Cost,
                OrderDate = DateTime.Now
            };
            _orderRepository.AddOrder(order);

        }

        public Order GetLastOrder(string userId)
        {
            var orders = _orderRepository.GetOrdersByUserId(userId);
            if (orders == null || !orders.Any())
            {
                throw new Exception("No orders found for this user.");
            }

            return orders.OrderByDescending(o => o.OrderDate).FirstOrDefault();
        }
    }
}
