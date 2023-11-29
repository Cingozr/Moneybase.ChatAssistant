using ChatAssistant.Infrastructure.Data.Dtos.ResponseDtos;
using MediatR;

namespace ChatAssistant.Application.Commands.DisconnectedSession
{
    public class DisconnectedSessionCommand : IRequest<ServiceResponseModel>
    {
        public string ConnectionId { get; set; }
    }
}
