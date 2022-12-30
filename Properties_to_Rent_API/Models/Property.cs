using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Properties_to_Rent_API.Models
{
    [BsonIgnoreExtraElements]
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]        
        public string Id { get; set; } = string.Empty;

        [BsonElement("Address")]
        public string Address { get; set; } =string.Empty;

        [BsonElement("city")]
        public string City { get; set; } =string.Empty;

        [BsonElement("state")]
        public string State { get; set; }=string.Empty;

        [BsonElement("zipcode")]
        public string ZipCode { get; set; }= string.Empty;

        [BsonElement("montlyprice")]
        public decimal MontlyPrice { get; set; }

        [BsonElement("availablefrom")]
        public string AvailableFrom { get; set; } = string.Empty;

        [BsonElement("bedrooms")]
        public int BedRooms { get; set; }

        [BsonElement("baths")]
        public int Baths { get; set; }

        [BsonElement("sqft")]
        public decimal SqFt { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("nroapplicants")]
        public int NroApplicants { get; set; }

    }
}
