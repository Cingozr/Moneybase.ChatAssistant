using ChatAssistant.Infrastructure.Data.Dtos.ResponseDtos;
using MediatR;

namespace ChatAssistant.Application.Commands.CreateChatSession
{
    public class CreateChatSessionCommand: IRequest<ServiceResponseModel>
    {
        public string RequestorConnectionId { get; set; }
    }
}
