using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Models
{
    public class ProductDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } 

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("subcategory")]
        public string Subcategory { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("stockCount")]
        public int StockCount { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; }

        [BsonElement("dateAdded")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)] // UTC formatını belirtir
        public DateTime DateAdded { get; set; } = DateTime.UtcNow; // Varsayılan değer olarak şu anki zaman

    }
}
