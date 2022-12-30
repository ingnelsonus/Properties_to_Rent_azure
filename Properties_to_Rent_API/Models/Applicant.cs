using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Properties_to_Rent_API.Models
{
    [BsonIgnoreExtraElements]
    public class Applicant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }=string.Empty;

        [BsonElement("name")]
        public string Name { get; set; }= string.Empty;

        [BsonElement("email")]
        public string Email { get; set; }=string.Empty ;

        [BsonElement("phone")]
        public string Phone { get; set; } = string.Empty;

        [BsonElement("address")]
        public string Address { get; set; } = string.Empty;

        [BsonElement("city")]
        public string City { get; set; } = string.Empty;

        [BsonElement("state")]
        public string State { get; set; } = string.Empty;

        [BsonElement("zipcode")]
        public string ZipCode { get; set; } = string.Empty;

    }
}
