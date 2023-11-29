using ChatAssistant.Infrastructure.Data.Dtos.RequestDtos.ChatSessions;
using ChatAssistant.Infrastructure.Data.Dtos.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace ChatAssistant.Infrastructure.ApiClients.Endpoints.ChatSessions
{
    public interface IChatSessionApi
    {
        [Post("/CreateSession")]
        Task<ServiceResponseModel> CreateSession([Body] CreateSessionRequestModel requestModel);

        [Post("/DisconnectedSession")]
        Task<ServiceResponseModel> DisconnectedSession([Body] DisconnectedSessionRequestModel requestModel);

        [Post("/CreatePollChat")]
        Task<ServiceResponseModel> CreatePollChat([FromBody] PollChatSessionRequestModel command);


    }
}
