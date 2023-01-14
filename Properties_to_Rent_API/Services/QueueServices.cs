using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

namespace Properties_to_Rent_API.Services
{
    public class QueueServices : IQueueServices
    {
        private readonly IConfiguration _config;

        public QueueServices(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendMessageAsync<T>(T ServicesBusMessage, string queueName)
        {
            var queueClient = new ServiceBusClient(_config.GetConnectionString("cnxRentPropertiesBus"));
            var sender = queueClient.CreateSender(queueName);
            var body = JsonSerializer.Serialize(ServicesBusMessage);
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(body));
            await sender.SendMessageAsync(message);
        }

    }
}
