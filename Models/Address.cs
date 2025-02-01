using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Models
{
    public class Address
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("FullName")]
        public string FullName { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("Street")]
        public string Street { get; set; }

        [BsonElement("City")]
        public string City { get; set; }

        [BsonElement("PostalCode")]
        public string PostalCode { get; set; }

        [BsonElement("Country")]
        public string Country { get; set; }
    }
}
