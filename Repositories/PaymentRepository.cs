using ECommerceApp.Data;
using ECommerceApp.Models;
using MongoDB.Driver;

namespace ECommerceApp.Repositories
{
    public class PaymentRepository
    {
        private readonly IMongoCollection<Payment> _paymentCollection;

        public PaymentRepository(MongoDbContext context)
        {
            _paymentCollection = context.GetCollection<Payment>("Payments");
        }

        public void AddPayment(Payment payment)
        {
            _paymentCollection.InsertOne(payment);
        }

        public Payment GetPaymentById(string id)
        {
            return _paymentCollection.Find(p => p.Id == id).FirstOrDefault();
        }
    }
}
