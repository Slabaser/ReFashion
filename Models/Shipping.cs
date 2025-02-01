using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Models
{
    public class Shipping
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("Method")]
        public string Method { get; set; }

        [BsonElement("Cost")]
        public decimal Cost { get; set; }

        [BsonElement("EstimatedDelivery")]
        public DateTime EstimatedDelivery { get; set; } // string yerine DateTime
    }
}
