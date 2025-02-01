using ECommerceApp.Data;
using ECommerceApp.Models;
using MongoDB.Driver;

namespace ECommerceApp.Repositories
{
    public class AddressRepository
    {
        private readonly IMongoCollection<Address> _addressCollection;

        public AddressRepository(MongoDbContext context)
        {
            _addressCollection = context.GetCollection<Address>("Addresses");
        }

        public void AddAddress(Address address)
        {
            _addressCollection.InsertOne(address);
        }

        public Address GetAddressById(string id)
        {
            return _addressCollection.Find(a => a.Id == id).FirstOrDefault();
        }
    }
}
