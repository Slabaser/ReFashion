using ECommerceApp.Data;
using ECommerceApp.Models;
using MongoDB.Driver;

namespace ECommerceApp.Repositories
{
    public class ShippingRepository
    {
        private readonly IMongoCollection<Shipping> _shippingCollection;

        public ShippingRepository(MongoDbContext context)
        {
            _shippingCollection = context.GetCollection<Shipping>("Shippings");
        }

        public void AddShipping(Shipping shipping)
        {
            _shippingCollection.InsertOne(shipping);
        }

        public Shipping GetShippingById(string id)
        {
            return _shippingCollection.Find(s => s.Id == id).FirstOrDefault();
        }

        public void UpdateShipping(string id, Shipping updatedShipping)
        {
            var filter = Builders<Shipping>.Filter.Eq(s => s.Id, id);
            _shippingCollection.ReplaceOne(filter, updatedShipping);
        }

        public List<Shipping> GetShippingsByUserId(string userId)
        {
            return _shippingCollection.Find(s => s.UserId == userId).ToList();
        }

    }
}
