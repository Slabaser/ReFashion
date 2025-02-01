using ECommerceApp.Models;
using ECommerceApp.Data;
using MongoDB.Driver;

namespace ECommerceApp.Repositories
{
    public class ReviewRepository
    {
        private readonly IMongoCollection<Review> _reviews;

        public ReviewRepository(MongoDbContext context)
        {
            _reviews = context.GetCollection<Review>("Reviews");
        }

        public void AddReview(Review review)
        {
            _reviews.InsertOne(review);
        }

        public List<Review> GetReviewsByProductId(string productId)
        {
            var filter = Builders<Review>.Filter.Eq(r => r.ProductId, productId);
            return _reviews.Find(filter).ToList();
        }
    }
}
