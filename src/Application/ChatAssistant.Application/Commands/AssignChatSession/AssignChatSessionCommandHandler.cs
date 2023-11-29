using ChatAssistant.Domain.Entities;
using ChatAssistant.Domain.Enums;
using ChatAssistant.Infrastructure.Data;
using ChatAssistant.Infrastructure.Data.Dtos.ResponseDtos;
using ChatAssistant.Infrastructure.Data.Repositories.Chats;
using ChatAssistant.Infrastructure.Data.Repositories.Teams;
using ChatAssistant.Infrastructure.Messaging.Services.RabbitMQ;
using LiveChatSupport.Infrastructure.Data.Dtos;
using MediatR;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace ChatAssistant.Application.Commands.AssignChatSession
{
    public class AssignChatSessionCommandHandler : IRequestHandler<AssignChatSessionCommand, ServiceResponseModel<ChatSession>>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly IRabbitMQClientService _rabbitMQClientService;
        private readonly ChatConfiguration _chatConfig;

        public AssignChatSessionCommandHandler(ITeamRepository teamRepository, IChatSessionRepository chatSessionRepository, IRabbitMQClientService rabbitMQClientService, IOptions<ChatConfiguration> chatConfig)
        {
            _chatConfig = chatConfig.Value;
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
            _chatSessionRepository = chatSessionRepository ?? throw new ArgumentNullException(nameof(chatSessionRepository));
            _rabbitMQClientService = rabbitMQClientService ?? throw new ArgumentNullException(nameof(rabbitMQClientService));
        }

        public async Task<ServiceResponseModel<ChatSession>> Handle(AssignChatSessionCommand request, CancellationToken cancellationToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var chatSession = DeserializeChatSession(request.Message);
            if (chatSession == null)
            {
                return CreateBadRequestResponse();
            };

            if (await IsChatSessionActive(chatSession, cancellationToken))
            {
                return CreateSuccessResponse(chatSession);
            }


            var activeTeam = await GetActiveTeam(cancellationToken);
            if (activeTeam == null)
            {
                return CreateServiceUnavailableResponse(chatSession);
            }

            var chatSessions = await _chatSessionRepository.GetAllAsync(cancellationToken);

            var assignedAgent = await AssignAgent(chatSession, activeTeam, chatSessions, cancellationToken);
            if (assignedAgent != null)
            {
                return CreateSuccessResponse(chatSession);
            }


            return CreateServiceUnavailableResponse(chatSession);
        }

        private ChatSession? DeserializeChatSession(string message)
        {
            return JsonSerializer.Deserialize<ChatSession>(message);
        }

        private async Task<bool> IsChatSessionActive(ChatSession chatSession, CancellationToken cancellationToken)
        {
            var existingSession = await _chatSessionRepository.GetByConnectionIdAsync(chatSession.ConnectionId, cancellationToken);
            return existingSession?.IsActive ?? false;
        }

        private async Task<Team?> GetActiveTeam(CancellationToken cancellationToken)
        {
            var teams = await _teamRepository.GetAllAsync(cancellationToken);
            return teams.FirstOrDefault(t => IsShiftActive(t));
        }

        private async Task<Agent?> AssignAgent(ChatSession chatSession, Team activeTeam, IEnumerable<ChatSession> chatSessions, CancellationToken cancellationToken)
        {

            var teams = await _teamRepository.GetAllAsync(cancellationToken);
            var availableAgent = GetAvailableAgent(activeTeam, chatSessions);
            if (availableAgent != null)
            {
                await AssignSessionToAgentAsync(chatSession, availableAgent, cancellationToken);
                return availableAgent;
            }

            if (IsQueueFull(activeTeam))
            {
                var overflowTeam = GetOverflowTeam(teams);
                var availableOverflowAgent = GetAvailableAgent(overflowTeam, chatSessions);
                if (availableOverflowAgent != null)
                {
                    await AssignSessionToAgentAsync(chatSession, availableOverflowAgent, cancellationToken);
                    return availableOverflowAgent;
                }
            }

            return null;
        }

        private ServiceResponseModel<ChatSession> CreateBadRequestResponse()
        {
            return new ServiceResponseModel<ChatSession>() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
        }

        private ServiceResponseModel<ChatSession> CreateSuccessResponse(ChatSession chatSession)
        {
            return new ServiceResponseModel<ChatSession>()
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Data = chatSession
            };
        }

        private ServiceResponseModel<ChatSession> CreateServiceUnavailableResponse(ChatSession chatSession)
        {
            return new ServiceResponseModel<ChatSession>() { IsSuccess = false, StatusCode = HttpStatusCode.ServiceUnavailable, Data = chatSession };
        }

        private bool IsShiftActive(Team team)
        {
            var currentHour = DateTime.Now.TimeOfDay.TotalHours;
            return team.Shift switch
            {
                Shift.Morning => currentHour >= ShiftTimings.ShiftStartTimeMorning && currentHour < ShiftTimings.ShiftEndTimeMorning,
                Shift.Afternoon => currentHour >= ShiftTimings.ShiftStartTimeAfternoon && currentHour < ShiftTimings.ShiftEndTimeAfternoon,
                Shift.Night => currentHour >= ShiftTimings.ShiftStartTimeNight || currentHour < ShiftTimings.ShiftEndTimeNight,
                _ => false,
            };
        }

        private Agent? GetAvailableAgent(Team team, IEnumerable<ChatSession> chatSessions)
        {
            return team?.Agents
                .Where(agent => IsAgentAvailable(agent, chatSessions))
                .OrderBy(agent => agent.Seniority)
                .ThenBy(agent => chatSessions.Count(cs => cs.AgentId == agent.Id && !cs.IsActive))
                .FirstOrDefault();
        }

        private async Task AssignSessionToAgentAsync(ChatSession chatSession, Agent agent, CancellationToken cancellationToken)
        {
            chatSession.AgentId = agent.Id;
            chatSession.IsAssigned = true;
            chatSession.IsActive = true;
            chatSession.LastPollTime = DateTime.UtcNow;

            await _chatSessionRepository.AddAsync(chatSession, cancellationToken);
        }

        private bool IsAgentAvailable(Agent agent, IEnumerable<ChatSession> chatSessions)
        {
            return chatSessions.Count(cs => cs.AgentId == agent.Id && cs.IsActive) < _chatConfig.MaxConcurrentChatsPerAgent;
        }

        private bool IsQueueFull(Team activeTeam)
        {
            var activeTeamCapacity = activeTeam?.Agents.Sum(agent => _chatConfig.MaxConcurrentChatsPerAgent * agent.Efficiency) ?? 0;
            return GetQueueLength() >= activeTeamCapacity * ShiftTimings.OverflowCapacityMultiplier;
        }

        private uint GetQueueLength()
        {
            return _rabbitMQClientService.GetMessageCount();
        }

        private Team? GetOverflowTeam(IEnumerable<Team> teams)
        {
            return teams.FirstOrDefault(t => t.TeamName.Equals("Overflow", StringComparison.OrdinalIgnoreCase));
        }

        public class ShiftTimings
        {
            public const double ShiftStartTimeMorning = 7;
            public const double ShiftEndTimeMorning = 15;
            public const double ShiftStartTimeAfternoon = 15;
            public const double ShiftEndTimeAfternoon = 23;
            public const double ShiftStartTimeNight = 23;
            public const double ShiftEndTimeNight = 7;
            public const double OverflowCapacityMultiplier = 1.5;
        }
    }
}
