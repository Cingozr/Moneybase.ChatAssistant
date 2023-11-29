using ChatAssistant.Infrastructure.ApiClients.Endpoints.ChatSessions;
using ChatAssistant.Infrastructure.Data.Dtos.RequestDtos.ChatSessions;
using Microsoft.AspNetCore.SignalR;

namespace ChatAssistant.Notification.Api.Hubs.Chat
{
    public class ChatNotificationHub : Hub<IChatNotificationClient>
    {
        private readonly IChatSessionApi _chatSessionApi;
        public ChatNotificationHub(IChatSessionApi chatSessionApi)
        {
            _chatSessionApi = chatSessionApi;
        } 

        public async Task RequestSupport()
        {
            var response = await _chatSessionApi.CreateSession(new CreateSessionRequestModel { RequestorConnectionId = Context.ConnectionId });
            await Clients.Client(Context.ConnectionId).NotifyChatSupportSent(response.Message);
        }

        public async Task Poll()
        {
            await _chatSessionApi.CreatePollChat(new PollChatSessionRequestModel { ConnectionId = Context.ConnectionId });
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await _chatSessionApi.DisconnectedSession(new DisconnectedSessionRequestModel { ConnectionId = Context.ConnectionId });
        }

    }

    public interface IChatNotificationClient
    { 
        Task NotifyChatSupportAssigned(string message);
        Task NotifyAgentsUnavailable(string message);
        Task NotifyChatSupportSent(string message);
    }
}
