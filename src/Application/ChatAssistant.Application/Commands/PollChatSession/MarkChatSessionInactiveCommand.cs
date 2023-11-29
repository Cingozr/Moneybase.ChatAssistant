using ChatAssistant.Infrastructure.Data.Dtos.ResponseDtos;
using MediatR;

namespace ChatAssistant.Application.Commands.PollChatSession
{
    public  class MarkChatSessionInactiveCommand : IRequest<ServiceResponseModel>
    {
        public string SessionId { get; set; }
    }
}
