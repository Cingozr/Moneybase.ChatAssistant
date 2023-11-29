namespace ChatAssistant.Infrastructure.Messaging.Services.RabbitMQ
{
    public interface IRabbitMQClientService
    {
        Task SendMessageAsync<T>(T message);
        void StartConsuming(Func<string, CancellationToken, Task<bool>> processMessageAsync, CancellationToken cancellationToken);
        uint GetMessageCount();
    }
}
