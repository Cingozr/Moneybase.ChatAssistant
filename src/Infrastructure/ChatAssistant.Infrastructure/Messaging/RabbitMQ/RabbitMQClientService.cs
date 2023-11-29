using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ChatAssistant.Infrastructure.Messaging.Services.RabbitMQ
{
    public class RabbitMQClientService : IRabbitMQClientService
    {
        private readonly IModel _channel;

        public RabbitMQClientService(IModel channel)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));

        }

        public void StartConsuming(Func<string, CancellationToken, Task<bool>> processMessageAsync, CancellationToken cancellationToken)
        {
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var success = await processMessageAsync(message, cancellationToken);

                if (success)
                {
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                else
                {
                    _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            _channel.BasicConsume(
                queue: RabbitMQConfiguration.ChatSessionQueueName,
                autoAck: false,
                consumer: consumer);
        }

        public uint GetMessageCount()
        {
            var result = _channel.QueueDeclarePassive(RabbitMQConfiguration.ChatSessionQueueName);
            return result.MessageCount;
        }

        public async Task SendMessageAsync<T>(T message)
        {
            var messageBody = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageBody);


            _channel.QueueDeclare(
              queue: RabbitMQConfiguration.ChatSessionQueueName,
              durable: true,
              exclusive: false,
              autoDelete: false,
              arguments: null);

            _channel.BasicPublish(
                exchange: RabbitMQConfiguration.ExchangeName,
                routingKey: RabbitMQConfiguration.ChatSessionRoutingKey,
                basicProperties: null,
                body: body);

            await Task.Yield();
        }
    }
}
