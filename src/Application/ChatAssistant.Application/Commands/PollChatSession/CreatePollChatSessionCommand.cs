using ChatAssistant.Infrastructure.Data.Dtos.ResponseDtos;
using MediatR;

namespace ChatAssistant.Application.Commands.PollChatSession
{
    public  class CreatePollChatSessionCommand : IRequest<ServiceResponseModel>
    {
        public string ConnectionId { get; set; }
    }
}
