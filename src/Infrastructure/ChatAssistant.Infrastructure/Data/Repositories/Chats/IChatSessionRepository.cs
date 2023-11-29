using ChatAssistant.Domain.Entities;

namespace ChatAssistant.Infrastructure.Data.Repositories.Chats
{
    public interface IChatSessionRepository
    {
        Task<ChatSession> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<ChatSession> GetByConnectionIdAsync(string connectionId, CancellationToken cancellationToken);
        Task<IEnumerable<ChatSession>> GetAllAsync(CancellationToken cancellationToken); 
        Task AddAsync(ChatSession session, CancellationToken cancellationToken);
        Task UpdateAsync(ChatSession session, CancellationToken cancellationToken);
        Task DeleteAsync(ChatSession session, CancellationToken cancellationToken);
        Task<int> AgentCurrentConcurrentCountAsync(int agentId, CancellationToken cancellationToken);

    }
}
