using ChatAssistant.Infrastructure.Data.Dtos.ResponseDtos;
using ChatAssistant.Infrastructure.Data.Repositories.Chats;
using MediatR;
using System.Net;

namespace ChatAssistant.Application.Queries.PollChatSession
{
    public class PollChatSessionQueryHandler : IRequestHandler<PollChatSessionQuery, ServiceResponseModel>
    {
        private readonly IChatSessionRepository _repository;

        public PollChatSessionQueryHandler(IChatSessionRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ServiceResponseModel> Handle(PollChatSessionQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = new ServiceResponseModel { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };


            try
            {
                var chatSession = await _repository.GetByConnectionIdAsync(request.SessionId, cancellationToken);
                if (chatSession != null)
                {
                    chatSession.LastPollTime = DateTime.UtcNow;
                    await _repository.UpdateAsync(chatSession, cancellationToken);
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
