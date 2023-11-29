using ChatAssistant.Infrastructure.Data.Dtos.ResponseDtos;
using ChatAssistant.Infrastructure.Data.Repositories.Chats;
using MediatR;
using System.Net;

namespace ChatAssistant.Application.Commands.PollChatSession
{
    public class MarkChatSessionInactiveCommandHandler : IRequestHandler<MarkChatSessionInactiveCommand, ServiceResponseModel>
    {
        private readonly IChatSessionRepository _chatRepository;
        public MarkChatSessionInactiveCommandHandler(IChatSessionRepository chatRepository)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        }
        public async Task<ServiceResponseModel> Handle(MarkChatSessionInactiveCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = new ServiceResponseModel { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
             

            try
            {
                var chatSession = await _chatRepository.GetByConnectionIdAsync(request.SessionId, cancellationToken);
                if (chatSession != null && (DateTime.UtcNow - chatSession.LastPollTime.GetValueOrDefault()).TotalSeconds > 3)
                {
                    chatSession.IsActive = false;
                    await _chatRepository.UpdateAsync(chatSession, cancellationToken);
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

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
