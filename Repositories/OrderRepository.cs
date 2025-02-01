using ECommerceApp.Data;
using ECommerceApp.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace ECommerceApp.Repositories
{
    public class OrderRepository
    {
        private readonly IMongoCollection<Order> _orderCollection;

        public OrderRepository(MongoDbContext context)
        {
            _orderCollection = context.GetCollection<Order>("Orders");
        }

        public void AddOrder(Order order)
        {
            _orderCollection.InsertOne(order);
        }

        public List<Order> GetOrdersByUserId(string userId)
        {
            return _orderCollection.Find(o => o.UserId == userId).ToList();
        }

        public Order GetOrderById(string id)
        {
            return _orderCollection.Find(o => o.Id == id).FirstOrDefault();
        }
    }
}
