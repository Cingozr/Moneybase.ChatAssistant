using ChatAssistant.Domain.Entities;
using ChatAssistant.Infrastructure.Data.Dtos.ResponseDtos;
using MediatR;

namespace ChatAssistant.Application.Commands.AssignChatSession
{
    public class AssignChatSessionCommand : IRequest<ServiceResponseModel<ChatSession>>
    {
        public string Message { get; set; }
    }
}
