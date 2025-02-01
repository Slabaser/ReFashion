using ECommerceApp.Models;
using ECommerceApp.Repositories;

namespace ECommerceApp.Services
{
    public class PaymentService
    {
        private readonly CartService _cartService;
        private readonly AddressRepository _addressRepository;
        private readonly PaymentRepository _paymentRepository;
        private readonly ShippingRepository _shippingRepository;
        private readonly OrderRepository _orderRepository;
        private readonly ProductRepository _productRepository;

        public PaymentService(
            CartService cartService,
            AddressRepository addressRepository,
            PaymentRepository paymentRepository,
            ShippingRepository shippingRepository,
            OrderRepository orderRepository,
            ProductRepository productRepository)
        {
            _cartService = cartService;
            _addressRepository = addressRepository;
            _paymentRepository = paymentRepository;
            _shippingRepository = shippingRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public void CompletePayment(string userId, PaymentViewModel model)
        {
            var cart = _cartService.GetCartByUserIdWithDetails(userId);

            if (cart == null || !cart.Items.Any())
            {
                throw new Exception("Your cart is empty.");
            }

            // Stok güncellemesi
            foreach (var item in cart.Items)
            {
                var product = _productRepository.GetProductById(item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Product not found: {item.ProductName}");
                }

                if (product.StockCount < item.Quantity)
                {
                    throw new Exception($"Insufficient stock for product: {product.Name}");
                }

                product.StockCount -= item.Quantity;
                _productRepository.UpdateProductStock(product);
            }

            // Ödeme kaydı
            var payment = new Payment
            {
                UserId = userId,
                PaymentMethod = model.PaymentMethod,
                CardNumber = "Not Stored", // Gerçek ödeme yapılmadığı için saklanmaz
                NameOnCard = model.NameOnCard,
                ExpirationDate = model.ExpirationDate,
                CVV = model.CVV
            };
            _paymentRepository.AddPayment(payment);



            // Sipariş kaydı
            var order = new Order
            {
                UserId = userId,

                Items = cart.Items,
                TotalAmount = model.GrandTotal,
                OrderDate = DateTime.Now,
                PaymentId = payment.Id,

            };
            _orderRepository.AddOrder(order);

            // Sepeti temizle
            _cartService.ClearCart(userId);
        }
    }
}
