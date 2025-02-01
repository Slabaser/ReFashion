using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace ECommerceApp.Models
{
    public class Wishlist
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] 
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("Items")]
        public List<WishlistItem> Items { get; set; } = new List<WishlistItem>();
    }

    public class WishlistItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("ProductId")]
        public string ProductId { get; set; }

        [BsonElement("ProductName")]
        public string ProductName { get; set; }

        [BsonElement("ProductPrice")]
        public decimal ProductPrice { get; set; }

        [BsonElement("AddedAt")]
        public DateTime AddedAt { get; set; }

        [BsonIgnore]
        public string ImageUrl { get; set; }
    }
}