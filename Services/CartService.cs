using ECommerceApp.Models;
using ECommerceApp.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ECommerceApp.Services
{
    public class CartService
    {
        private readonly CartRepository _cartRepository;
        private readonly ProductRepository _productRepository;

        public CartService(CartRepository cartRepository, ProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public Cart GetCartByUserIdWithDetails(string userId)
        {
            var cart = _cartRepository.GetCartByUserIdWithDetails(userId);

            if (cart == null || cart.Items == null || !cart.Items.Any())
            {
                Console.WriteLine($"Cart is null or empty for userId: {userId}");
                return new Cart // Boş bir cart döndürüyoruz
                {
                    UserId = userId,
                    Items = new List<CartItem>()
                };
            }

            foreach (var item in cart.Items)
            {
                var product = _productRepository.GetProductById(item.ProductId);
                if (product != null)
                {
                    item.ProductName = product.Name;
                    item.ProductPrice = product.Price;
                    item.ImageUrl = product.ImageUrl;
                }
            }

            return cart;
        }




        public void AddToCart(string userId, string productId, string productSize, int quantity)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(productId))
            {
                throw new ArgumentException("User ID or Product ID is missing.");
            }

            var cart = _cartRepository.GetCartByUserIdWithDetails(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Items = new List<CartItem>()
                };
                _cartRepository.CreateCart(cart);
            }

            var product = _productRepository.GetProductById(productId);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId && i.ProductSize == productSize);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    Id = ObjectId.GenerateNewId().ToString(), // Benzersiz bir ID oluşturuluyor
                    ProductId = productId,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    ProductSize = productSize,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                };

                cart.Items.Add(newItem);
            }

            _cartRepository.UpdateCart(cart.Id, cart);
        }

        public void RemoveFromCart(string userId, string productId)
        {
            _cartRepository.RemoveFromCart(userId, productId);
        }


        public void RemoveFromCartByProductId(string userId, string productId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(productId))
            {
                throw new ArgumentException("User ID or Product ID is missing.");
            }

            _cartRepository.RemoveItemFromCartByProductId(userId, productId);
        }


        public void UpdateCartItemQuantity(string userId, string productId, int quantity)
        {
            if (quantity > 10)
            {
                throw new Exception("Quantity cannot exceed 10.");
            }

            _cartRepository.UpdateCartItemQuantity(userId, productId, quantity);
        }


        public void UpdateCartItemQuantityById(string userId, string cartItemId, int quantity)
        {
            var cart = _cartRepository.GetCartByUserIdWithDetails(userId);
            if (cart == null || cart.Items == null)
            {
                throw new Exception("Cart not found.");
            }

            var itemToUpdate = cart.Items.FirstOrDefault(i => i.Id == cartItemId);
            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = quantity;
                _cartRepository.UpdateCart(cart.Id, cart);
            }
        }

        public void ClearCart(string userId)
        {
            var cart = _cartRepository.GetCartByUserIdWithDetails(userId);
            if (cart != null)
            {
                cart.Items.Clear();
                _cartRepository.UpdateCart(cart.Id, cart);
            }
        }


        public List<Cart> GetAllCarts()
        {
            return _cartRepository.GetAllCarts();
        }
    }
}
