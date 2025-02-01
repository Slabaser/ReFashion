using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Models
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("PaymentMethod")]
        public string PaymentMethod { get; set; } // e.g., "Credit Card", "PayPal"

        [BsonElement("CardNumber")]
        public string CardNumber { get; set; } // Masked or encrypted

        [BsonElement("NameOnCard")]
        public string NameOnCard { get; set; }

        [BsonElement("ExpirationDate")]
        public string ExpirationDate { get; set; } // MM/YY

        [BsonElement("CVV")]
        public string CVV { get; set; } // Optional; avoid storing raw CVVs
    }
}
