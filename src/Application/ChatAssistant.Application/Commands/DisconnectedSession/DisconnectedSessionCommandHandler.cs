using ChatAssistant.Infrastructure.Data.Dtos.ResponseDtos;
using ChatAssistant.Infrastructure.Data.Repositories.Chats;
using MediatR;
using System.Net;

namespace ChatAssistant.Application.Commands.DisconnectedSession
{

    public class DisconnectedSessionCommandHandler : IRequestHandler<DisconnectedSessionCommand, ServiceResponseModel>
    {
        private readonly IChatSessionRepository _chatSessionRepository;

        public DisconnectedSessionCommandHandler(IChatSessionRepository chatSessionRepository)
        {
            _chatSessionRepository = chatSessionRepository ?? throw new ArgumentNullException(nameof(chatSessionRepository));
        }

        public async Task<ServiceResponseModel> Handle(DisconnectedSessionCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = new ServiceResponseModel { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

             
            try
            {
                var chatSession = await _chatSessionRepository.GetByConnectionIdAsync(request.ConnectionId, cancellationToken);
                if (chatSession is null)
                {
                    return response;
                }

                chatSession.IsActive = false;
                await _chatSessionRepository.UpdateAsync(chatSession, cancellationToken);

                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Message = $"An error occured";
                response.AggregatedUFExceptions = ex.ToString();
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.OK; throw;
            }

            return response;
        }
    }
}
