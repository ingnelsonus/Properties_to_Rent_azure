using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Authentication;

namespace Properties_to_Rent_API.Repository
{
    public class MongoDBRepository
    {
        public MongoClient client;
        public IMongoDatabase db;

        public MongoDBRepository(string cnx,string dbName)
        {
            //client = new MongoClient(cnx);            
            MongoClientSettings sett = MongoClientSettings.FromUrl(
              new MongoUrl(cnx)
            );
            sett.UseTls = true;
            sett.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            client = new MongoClient(sett);

            var database = client.GetDatabase(dbName);
            bool isMongoLive = database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);

            db =client.GetDatabase(dbName);
        }
    }
}
