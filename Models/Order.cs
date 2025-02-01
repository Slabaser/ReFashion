using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ECommerceApp.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }



        [BsonElement("PaymentId")]
        public string PaymentId { get; set; }

        [BsonElement("Items")]
        public List<CartItem> Items { get; set; }

        [BsonElement("TotalAmount")]
        public decimal TotalAmount { get; set; }

        [BsonElement("OrderDate")]
        public DateTime OrderDate { get; set; }
    }
}
