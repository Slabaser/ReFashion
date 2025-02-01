using ECommerceApp.Models;
using ECommerceApp.Data;
using MongoDB.Driver;
using System.Collections.Generic;

namespace ECommerceApp.Services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryService(MongoDbContext dbContext)
        {
            _categories = dbContext.Categories;
        }

        // Sadece Men ve Women kategorilerini getir
        public Category GetCategoryByName(string name)
        {
            return _categories.Find(c => c.Name == name).FirstOrDefault();
        }

    }
}
