using ChatAssistant.Infrastructure.Data.Dtos.ResponseDtos;
using MediatR;

namespace ChatAssistant.Application.Queries.PollChatSession
{
    public class PollChatSessionQuery : IRequest<ServiceResponseModel>
    {
        public string SessionId { get; set; }
    }
}
