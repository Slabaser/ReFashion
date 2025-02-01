using ECommerceApp.Models;
using ECommerceApp.Data;
using MongoDB.Driver;
using System.Collections.Generic;

namespace ECommerceApp.Repositories
{
    public class CategoryRepository
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryRepository(MongoDbContext dbContext)
        {
            _categories = dbContext.GetCollection<Category>("Categories");
        }

        public List<Category> GetAllCategories()
        {
            return _categories.Find(_ => true).ToList();
        }
    }
}
