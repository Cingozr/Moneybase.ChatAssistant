using ChatAssistant.Notification.Api.Hubs.Chat;
using Microsoft.AspNetCore.SignalR;

namespace ChatAssistant.Notification.Api.BackgroundServices
{
    public class ServerTimeNotifier : BackgroundService
    {
        private static readonly TimeSpan Period = TimeSpan.FromSeconds(1);
        private readonly ILogger<ServerTimeNotifier> _logger;
        private readonly IHubContext<ChatNotificationHub, IChatNotificationClient> _context;

        public ServerTimeNotifier(ILogger<ServerTimeNotifier> logger, IHubContext<ChatNotificationHub, IChatNotificationClient> context)
        {
            _logger = logger;
            _context = context;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            using var timer = new PeriodicTimer(Period);

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                var datetime = DateTime.Now;
                _logger.LogInformation($"Executing {nameof(ServerTimeNotifier)} | {datetime}");
            }

        }
    }
}
