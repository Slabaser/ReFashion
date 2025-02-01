using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Models
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("UserFullName")]
        public string UserFullName { get; set; }

        [BsonElement("Comment")]
        public string Comment { get; set; }

        [BsonElement("Rating")]
        public int Rating { get; set; }

        [BsonElement("DateAdded")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime DateAdded { get; set; }

        [BsonElement("ProductId")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull] 
        public string ProductId { get; set; }
    }
}
