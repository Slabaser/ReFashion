using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ECommerceApp.Models
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("Items")]
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }

    public class CartItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } 

        [BsonElement("ProductId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }

        [BsonElement("ProductName")]
        public string ProductName { get; set; }

        [BsonElement("ProductPrice")]
        public decimal ProductPrice { get; set; }

        [BsonElement("ProductSize")]
        public string ProductSize { get; set; }

        [BsonElement("Quantity")]
        public int Quantity { get; set; }

        [BsonIgnore]
        public string ImageUrl { get; set; }
    }
}
