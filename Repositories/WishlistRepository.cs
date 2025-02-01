using ECommerceApp.Models;
using ECommerceApp.Data;
using MongoDB.Driver;
using MongoDB.Bson;

namespace ECommerceApp.Repositories
{
    public class WishlistRepository
    {
        private readonly IMongoCollection<Wishlist> _wishlistCollection;

        public WishlistRepository(MongoDbContext context)
        {
            _wishlistCollection = context.GetCollection<Wishlist>("Wishlists");
        }

        public Wishlist GetWishlistByUserId(string userId)
        {
            return _wishlistCollection.Find(w => w.UserId == userId).FirstOrDefault();
        }

        public IEnumerable<BsonDocument> Aggregate(BsonDocument[] pipeline)
        {
            return _wishlistCollection.Aggregate<BsonDocument>(pipeline).ToEnumerable();
        }

        public void CreateWishlist(Wishlist wishlist)
        {
            _wishlistCollection.InsertOne(wishlist);
        }

        public void AddItemToWishlist(string userId, WishlistItem item)
        {
            var filter = Builders<Wishlist>.Filter.Eq(w => w.UserId, userId);
            var update = Builders<Wishlist>.Update.Push(w => w.Items, item);

            var existingWishlist = GetWishlistByUserId(userId);
            if (existingWishlist == null)
            {
                var newWishlist = new Wishlist
                {
                    UserId = userId,
                    Items = new List<WishlistItem> { item }
                };
                CreateWishlist(newWishlist);
            }
            else
            {
                _wishlistCollection.UpdateOne(filter, update);
            }
        }


        public void RemoveItemFromWishlist(string userId, string productId)
        {
            var filter = Builders<Wishlist>.Filter.Eq(w => w.UserId, userId);
            var update = Builders<Wishlist>.Update.PullFilter(w => w.Items, i => i.ProductId == productId);

            // Güncelleme işlemi
            var result = _wishlistCollection.UpdateOne(filter, update);

            // Hata kontrolü
            if (result.ModifiedCount == 0)
            {
                Console.WriteLine("No matching wishlist item found to remove.");
                throw new Exception("Item not found in wishlist.");
            }
        }

    }
}
