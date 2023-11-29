using ChatAssistant.Domain.Entities;
using ChatAssistant.Infrastructure.Data.Dtos.ResponseDtos;
using ChatAssistant.Infrastructure.Messaging.Services.RabbitMQ;
using MediatR;
using System.Net;

namespace ChatAssistant.Application.Commands.CreateChatSession
{
    internal class CreateChatSessionCommandHandler : IRequestHandler<CreateChatSessionCommand, ServiceResponseModel>
    {
        private readonly IRabbitMQClientService _messagePublisher;

        public CreateChatSessionCommandHandler(IRabbitMQClientService messagePublisher)
        {
            _messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));
        }
        public async Task<ServiceResponseModel> Handle(CreateChatSessionCommand request, CancellationToken cancellationToken)
        {
            var response = new ServiceResponseModel { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
            try
            {
                var chatSession = new ChatSession
                {
                    ConnectionId = request.RequestorConnectionId,
                    QueueTime = DateTime.UtcNow,
                    IsAssigned = false,
                    IsActive = true
                };

                await _messagePublisher.SendMessageAsync(chatSession);
                 

                return new ServiceResponseModel()
                {
                    Message = "Your request has been received successfully.",
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                };

            }
            catch (Exception ex)
            {
                response.Message = $"An error occurred while creating the chat session.";
                response.AggregatedUFExceptions = ex.ToString();
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
            }

            return response;
        }

    }
}
