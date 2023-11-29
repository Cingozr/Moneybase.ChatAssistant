namespace ChatAssistant.Infrastructure.Messaging.Services.RabbitMQ
{
    public static class RabbitMQConfiguration
    {
        public const string HostName = "localhost";
        public const string ExchangeName = "";

        #region Roting Key
        public const string  ChatSessionRoutingKey = "chat-session-queue";
        #endregion


        #region Queue
        public const string ChatSessionQueueName = "chat-session-queue";
        #endregion
    }
}
