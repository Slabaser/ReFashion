using ECommerceApp.Models;
using ECommerceApp.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ECommerceApp.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        

        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Wishlist> Wishlists => _database.GetCollection<Wishlist>("Wishlists");
        public IMongoCollection<Cart> Carts => _database.GetCollection<Cart>("Carts");
        public IMongoCollection<Review> Reviews => _database.GetCollection<Review>("Reviews");
        public IMongoCollection<ProductDetail> Products => _database.GetCollection<ProductDetail>("Products");
        public IMongoCollection<Address> Addresses => _database.GetCollection<Address>("Addresses");
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
        public IMongoCollection<Payment> Payments => _database.GetCollection<Payment>("Payments");
        public IMongoCollection<Shipping> Shippings => _database.GetCollection<Shipping>("Shippings");
    }
}
