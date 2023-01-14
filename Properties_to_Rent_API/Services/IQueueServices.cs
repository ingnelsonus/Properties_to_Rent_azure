
namespace Properties_to_Rent_API.Services
{
    public interface IQueueServices
    {
        Task SendMessageAsync<T>(T ServicesBusMessage, string queueName);
    }
}