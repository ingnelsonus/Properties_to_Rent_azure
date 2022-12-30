using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Properties_to_Rent_API.Models
{
    [BsonIgnoreExtraElements]
    public class Application
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("applicant")]
        public string Applicant { get; set; } = string.Empty;

        [BsonElement("property")]
        public string Property { get; set; } = string.Empty;

        [BsonElement("applicationdate")]
        public string ApplicationDate { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = string.Empty;

    }
}
