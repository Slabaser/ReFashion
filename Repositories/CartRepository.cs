using ECommerceApp.Models;
using ECommerceApp.Data;
using MongoDB.Driver;

namespace ECommerceApp.Repositories
{
    public class CartRepository
    {
        private readonly IMongoCollection<Cart> _cartCollection;

        public CartRepository(MongoDbContext context)
        {
            _cartCollection = context.GetCollection<Cart>("Carts");
        }

        public Cart GetCartByUserIdWithDetails(string userId)
        {
            return _cartCollection.Find(c => c.UserId == userId).FirstOrDefault();
        }

        public void CreateCart(Cart cart)
        {
            _cartCollection.InsertOne(cart);
        }

        public void AddItemToCart(string userId, CartItem item)
        {
            var filter = Builders<Cart>.Filter.Eq(c => c.UserId, userId);
            var update = Builders<Cart>.Update.Push(c => c.Items, item);

            var existingCart = GetCartByUserIdWithDetails(userId);
            if (existingCart == null)
            {
                var newCart = new Cart
                {
                    UserId = userId,
                    Items = new List<CartItem> { item }
                };
                CreateCart(newCart);
            }
            else
            {
                _cartCollection.UpdateOne(filter, update);
            }
        }

        public void RemoveFromCart(string userId, string productId)
        {
            var filter = Builders<Cart>.Filter.And(
                Builders<Cart>.Filter.Eq(c => c.UserId, userId),
                Builders<Cart>.Filter.ElemMatch(c => c.Items, item => item.ProductId == productId)
            );

            var update = Builders<Cart>.Update.PullFilter(c => c.Items, item => item.ProductId == productId);

            var result = _cartCollection.UpdateOne(filter, update);

            if (result.ModifiedCount == 0)
            {
                Console.WriteLine($"No items were removed from the cart for User ID: {userId} and Product ID: {productId}");
            }
            else
            {
                Console.WriteLine($"Item with Product ID: {productId} was successfully removed from the cart for User ID: {userId}");
            }
        }

        public void RemoveItemFromCartByProductId(string userId, string productId)
        {
            var filter = Builders<Cart>.Filter.Eq(c => c.UserId, userId);
            var update = Builders<Cart>.Update.PullFilter(c => c.Items, i => i.ProductId == productId);

            var result = _cartCollection.UpdateOne(filter, update);

            Console.WriteLine($"Matched Count: {result.MatchedCount}, Modified Count: {result.ModifiedCount}");
            if (result.MatchedCount == 0)
            {
                throw new Exception("Cart not found for the specified user.");
            }
        }


        public void UpdateCartItemQuantity(string userId, string productId, int quantity)
        {
            var filter = Builders<Cart>.Filter.And(
                Builders<Cart>.Filter.Eq(c => c.UserId, userId),
                Builders<Cart>.Filter.ElemMatch(c => c.Items, i => i.ProductId == productId)
            );
            var update = Builders<Cart>.Update.Set("Items.$.Quantity", quantity);

            _cartCollection.UpdateOne(filter, update);
        }


        public void UpdateCartItemQuantityById(string userId, string cartItemId, int quantity)
        {
            var filter = Builders<Cart>.Filter.And(
                Builders<Cart>.Filter.Eq(c => c.UserId, userId),
                Builders<Cart>.Filter.ElemMatch(c => c.Items, i => i.Id == cartItemId)
            );
            var update = Builders<Cart>.Update.Set("Items.$.Quantity", quantity);

            _cartCollection.UpdateOne(filter, update);
        }

        public void UpdateCart(string cartId, Cart cart)
        {
            var filter = Builders<Cart>.Filter.Eq(c => c.Id, cartId);
            _cartCollection.ReplaceOne(filter, cart);
        }

        public void ClearCart(string userId)
        {
            var filter = Builders<Cart>.Filter.Eq(c => c.UserId, userId);
            _cartCollection.DeleteOne(filter);
        }

        public List<Cart> GetAllCarts()
        {
            return _cartCollection.Find(_ => true).ToList();
        }
    }
}
