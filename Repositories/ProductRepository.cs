using ECommerceApp.Models;
using ECommerceApp.Data;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;

namespace ECommerceApp.Repositories
{
    public class ProductRepository
    {
        private readonly IMongoCollection<ProductDetail> _products;

        public ProductRepository(MongoDbContext context)
        {
            _products = context.GetCollection<ProductDetail>("Products");
        }

        // Kategoriye göre ürünleri getirir
        public List<ProductDetail> GetProductsByCategory(string category)
        {
            return _products.Find(p => p.Category == category).ToList();
        }

        // Alt kategoriye göre ürünleri getirir
        public List<ProductDetail> GetProductsBySubcategory(string category, string subcategory)
        {
            var filter = Builders<ProductDetail>.Filter.And(
                Builders<ProductDetail>.Filter.Eq(p => p.Category, category),
                Builders<ProductDetail>.Filter.Eq(p => p.Subcategory, subcategory)
            );
            return _products.Find(filter).ToList();
        }

        // ID'ye göre ürün getir
        public ProductDetail GetProductById(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("Invalid ObjectId format.");
            }

            var filter = Builders<ProductDetail>.Filter.Eq(p => p.Id, objectId.ToString());
            return _products.Find(filter).FirstOrDefault();
        }

        // Atlas Search ile arama fonksiyonu
        public List<ProductDetail> SearchProducts(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                var filter = Builders<ProductDetail>.Filter.Or(
                    Builders<ProductDetail>.Filter.Regex("Name", new BsonRegularExpression(query, "i")),
                    Builders<ProductDetail>.Filter.Regex("Description", new BsonRegularExpression(query, "i")),
                    Builders<ProductDetail>.Filter.Regex("Category", new BsonRegularExpression(query, "i")),
                    Builders<ProductDetail>.Filter.Regex("Subcategory", new BsonRegularExpression(query, "i"))
                );

                return _products.Find(filter).Limit(10).ToList();
            }

            return new List<ProductDetail>();
        }
        public void UpdateProduct(string id, ProductDetail updatedProduct)
        {
            var filter = Builders<ProductDetail>.Filter.Eq(p => p.Id, id);
            _products.ReplaceOne(filter, updatedProduct);
        }

        // Stok güncelleme
        public void UpdateProductStock(ProductDetail product)
        {
            var filter = Builders<ProductDetail>.Filter.Eq(p => p.Id, product.Id);
            var update = Builders<ProductDetail>.Update.Set(p => p.StockCount, product.StockCount);
            _products.UpdateOne(filter, update);
        }

        public List<ProductDetail> GetNewArrivals(int days = 10)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return _products.Find(p => p.DateAdded >= cutoffDate).SortByDescending(p => p.DateAdded).ToList();
        }



    }
}
