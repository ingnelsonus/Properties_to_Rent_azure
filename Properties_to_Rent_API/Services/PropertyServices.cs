using Properties_to_Rent_API.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Authentication;
using Properties_to_Rent_API.Repository;

namespace Properties_to_Rent_API.Services
{
    public class PropertyServices : IPropertyServices
    {
        internal MongoDBRepository _repository;
        private readonly IMongoCollection<Property> _properties;

        public PropertyServices(IPropertiesDbSettings settings)
        {
            _repository = new MongoDBRepository(settings.ConnectionString, settings.DataBaseName);

            ////Azure mongoDB connect.
            //string connectionString = settings.ConnectionString;
            //MongoClientSettings sett = MongoClientSettings.FromUrl(
            //  new MongoUrl(connectionString)
            //);
            //sett.UseTls = true;            
            //sett.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            //var client = new MongoClient(sett);

            //var database = client.GetDatabase(settings.DataBaseName);
            //bool isMongoLive = database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);


            ////Local Connect.
            //var client = new MongoClient("mongodb://localhost:27017");
            //var state = client.Cluster.Description.State;

            //var database = client.GetDatabase("protertydb");
            //bool isMongoLive = database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);

            _properties = _repository.db.GetCollection<Property>(settings.PropertyCollectionName);
        }

        public Property Create(Property property)
        {
            _properties.InsertOne(property);
            return property;
        }

        public List<Property> GetAll()
        {            
            var properties = _properties.Find(prop=>true).ToList();
            return properties;

        }

        public Property GetByID(string id)
        {
            return _properties.Find(property => property.Id==id).FirstOrDefault();
        }

        public List<Property> GetPropertiesByCity(string city)
        {
            return _properties.Find(property => property.City == city).ToList();
        }

        public void Remove(string id)
        {
            _properties.DeleteOne(property => property.Id == id);
        }

        public void Update(string id, Property property)
        {
            _properties.ReplaceOne(property => property.Id == id,property);
        }
    }
}
