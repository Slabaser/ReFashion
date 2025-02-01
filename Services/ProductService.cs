using ECommerceApp.Models;
using ECommerceApp.Data;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using ECommerceApp.Repositories;

namespace ECommerceApp.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<ProductDetail> _products;
        private readonly ProductRepository _productRepository;

        public ProductService(MongoDbContext dbContext)
        {
            // MongoDB'deki "Products" koleksiyonunu bağlama
            _products = dbContext.GetCollection<ProductDetail>("Products");
        }

        // Tüm ürünleri getir
        public List<ProductDetail> GetAllProducts()
        {
            return _products.Find(_ => true).ToList();
        }
        
        // Kategoriye göre ürünleri getir
        public List<ProductDetail> GetProductsByCategory(string category)
        {
            return _products.Find(p => p.Category == category).ToList();
        }

        // Alt kategoriye göre ürünleri getir
        public List<ProductDetail> GetProductsBySubcategory(string category, string subcategory)
        {
            var filter = Builders<ProductDetail>.Filter.And(
                Builders<ProductDetail>.Filter.Eq(p => p.Category, category),
                Builders<ProductDetail>.Filter.Eq(p => p.Subcategory, subcategory)
            );
            return _products.Find(filter).ToList();
        }

        // Ürün ID'sine göre tek bir ürün getir
        public ProductDetail GetProductById(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                Console.WriteLine("Invalid ObjectId format");
                return null; // Geçersiz bir ID formatıysa null döndür
            }

            var filter = Builders<ProductDetail>.Filter.Eq(p => p.Id, objectId.ToString());
            return _products.Find(filter).FirstOrDefault();
        }

        // Yeni ürün ekle
        public void AddProduct(ProductDetail product)
        {
            _products.InsertOne(product);
        }

        // Ürünü güncelle
        public void UpdateProduct(string id, ProductDetail updatedProduct)
        {
            if (ObjectId.TryParse(id, out var objectId))
            {
                var filter = Builders<ProductDetail>.Filter.Eq("_id", objectId);
                _products.ReplaceOne(filter, updatedProduct);
            }
            else
            {
                Console.WriteLine($"Invalid ObjectId format for id: {id}");
            }
        }

        // Ürünü sil
        public void DeleteProduct(string id)
        {
            if (ObjectId.TryParse(id, out var objectId))
            {
                var filter = Builders<ProductDetail>.Filter.Eq("_id", objectId);
                _products.DeleteOne(filter);
            }
            else
            {
                Console.WriteLine($"Invalid ObjectId format for id: {id}");
            }
        }

        public List<ProductDetail> GetProductsByCategoryAndSubcategoryWithoutCurrent(string category, string subcategory, string currentProductId)
        {
            if (!ObjectId.TryParse(currentProductId, out var currentObjectId))
            {
                Console.WriteLine($"Invalid ObjectId format for currentProductId: {currentProductId}");
                return new List<ProductDetail>();
            }

            var filter = Builders<ProductDetail>.Filter.And(
                Builders<ProductDetail>.Filter.Eq(p => p.Category, category),  // Kategori eşleşmeli
                Builders<ProductDetail>.Filter.Eq(p => p.Subcategory, subcategory), // Alt kategori eşleşmeli
                Builders<ProductDetail>.Filter.Ne("_id", currentObjectId) // Şu anki ürünü hariç tut
            );

            return _products.Find(filter).Limit(4).ToList(); // Maksimum 4 ürün getir
        }


        // Arama fonksiyonu
        public List<ProductDetail> SearchProducts(string query, string category = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new List<ProductDetail>();
            }

            // Query için genel filtre
            var filter = Builders<ProductDetail>.Filter.Or(
                Builders<ProductDetail>.Filter.Regex("Name", new BsonRegularExpression(query, "i")),
                Builders<ProductDetail>.Filter.Regex("Description", new BsonRegularExpression(query, "i")),
                Builders<ProductDetail>.Filter.Regex("Category", new BsonRegularExpression(query, "i")),
                Builders<ProductDetail>.Filter.Regex("Subcategory", new BsonRegularExpression(query, "i"))
            );

            // Kategori varsa ek filtre uygula
            if (!string.IsNullOrEmpty(category))
            {
                var categoryFilter = Builders<ProductDetail>.Filter.Eq(p => p.Category, category);
                filter = Builders<ProductDetail>.Filter.And(filter, categoryFilter);
            }

            // Filtreyi uygula
            return _products.Find(filter).ToList();
        }

        // Stok güncelleme
        public void UpdateStock(string productId, int quantity)
        {
            if (!ObjectId.TryParse(productId, out var objectId))
            {
                Console.WriteLine($"Invalid ObjectId format for productId: {productId}");
                return;
            }

            var filter = Builders<ProductDetail>.Filter.Eq(p => p.Id, productId);
            var update = Builders<ProductDetail>.Update.Inc(p => p.StockCount, -quantity);

            var result = _products.UpdateOne(filter, update);

            if (result.MatchedCount == 0)
            {
                Console.WriteLine($"No product found with ID: {productId}");
            }
            else if (result.ModifiedCount == 0)
            {
                Console.WriteLine($"Failed to update stock for product with ID: {productId}");
            }
        }

        public List<ProductDetail> GetNewArrivals(int days = 10)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow.AddDays(-days);
                return _products.Find(p => p.DateAdded >= cutoffDate).SortByDescending(p => p.DateAdded).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching new arrivals: {ex.Message}");
                return new List<ProductDetail>();
            }
        }


    }
}
