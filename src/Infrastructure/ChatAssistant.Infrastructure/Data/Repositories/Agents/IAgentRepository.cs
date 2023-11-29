namespace ChatAssistant.Infrastructure.Data.Repositories.Agents
{
    public interface IAgentRepository
    {
        Task GetTask(string agentId);
    }
}
