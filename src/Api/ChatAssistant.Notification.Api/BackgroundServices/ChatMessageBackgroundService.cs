using ChatAssistant.Application.Commands.AssignChatSession;
using ChatAssistant.Domain.Entities;
using ChatAssistant.Infrastructure.Data.Dtos.ResponseDtos;
using ChatAssistant.Infrastructure.Messaging.Services.RabbitMQ;
using ChatAssistant.Notification.Api.Hubs.Chat;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

public class ChatMessageBackgroundService : BackgroundService
{
    private const int PollingDelayInSeconds = 1;

    private readonly ILogger<ChatMessageBackgroundService> _logger;
    private readonly IRabbitMQClientService _rabbitMQClientService;
    private readonly IMediator _mediator;
    private readonly IHubContext<ChatNotificationHub, IChatNotificationClient> _chatNotificationContext;
    private readonly TimeSpan _pollingDelay = TimeSpan.FromSeconds(PollingDelayInSeconds);

    public ChatMessageBackgroundService(
        ILogger<ChatMessageBackgroundService> logger,
        IRabbitMQClientService rabbitMQClientService,
        IServiceScopeFactory serviceScopeFactory,
        IMediator mediator,
        IHubContext<ChatNotificationHub, IChatNotificationClient> chatNotificationContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _rabbitMQClientService = rabbitMQClientService ?? throw new ArgumentNullException(nameof(rabbitMQClientService));
        var serviceScope = serviceScopeFactory.CreateScope();
        _mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>() ?? throw new InvalidOperationException("Mediator service is not available.");
        _chatNotificationContext = chatNotificationContext ?? throw new ArgumentNullException(nameof(chatNotificationContext));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _rabbitMQClientService.StartConsuming(ProcessMessageAsync, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_pollingDelay, stoppingToken);
        }
    }

    private async Task<bool> ProcessMessageAsync(string message, CancellationToken stoppingToken)
    {
        if (string.IsNullOrEmpty(message)) return false;

        try
        { 

            var response = await _mediator.Send(new AssignChatSessionCommand { Message = message }, stoppingToken); 

            await NotifyClients(response.Data, response);

            return response.IsSuccess;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing message.");
            return false;
        }
    }

    private async Task NotifyClients(ChatSession chatSession, ServiceResponseModel response)
    {
        if (response.IsSuccess)
        {
            response.Message = $"Your session({chatSession.ConnectionId}) was assigned to an agent... Please wait...";
            await _chatNotificationContext.Clients.Client(chatSession.ConnectionId).NotifyChatSupportAssigned(JsonSerializer.Serialize(response));
        }

        else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
        { 
            response.Message = $"All agents are currently busy please try again later...";
            await _chatNotificationContext.Clients.Client(chatSession.ConnectionId).NotifyAgentsUnavailable(JsonSerializer.Serialize(response));
        }
    }
}
