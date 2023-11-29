
namespace ChatAssistant.Infrastructure.Data.Repositories.Agents
{
    public class AgentRepository : IAgentRepository
    {
        private readonly ChatAssistantDbContext _context;

        public AgentRepository(ChatAssistantDbContext context)
        {
            _context = context;
        }
        public Task GetTask(string agentId)
        {
            _context.Agents.Count();
            return Task.FromResult(0);
        }
    }
}
