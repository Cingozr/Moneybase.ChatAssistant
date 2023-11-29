using ChatAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace ChatAssistant.Infrastructure.Data.Repositories.Chats
{
    public class ChatSessionRepository : IChatSessionRepository
    {
        private readonly ChatAssistantDbContext _context;

        public ChatSessionRepository(ChatAssistantDbContext context)
        {
            _context = context;
        }

        public async Task<ChatSession> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.ChatSessions.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<ChatSession>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.ChatSessions.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task AddAsync(ChatSession session, CancellationToken cancellationToken)
        {
            _context.ChatSessions.Add(session);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(ChatSession session, CancellationToken cancellationToken)
        {
            _context.Entry(session).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(ChatSession session, CancellationToken cancellationToken)
        {
            _context.ChatSessions.Remove(session);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> AgentCurrentConcurrentCountAsync(int agentId, CancellationToken cancellationToken)
        {
            return await _context.ChatSessions.CountAsync(x => x.AgentId == agentId && x.IsAssigned && x.IsActive, cancellationToken);
        }

        public async Task<ChatSession> GetByConnectionIdAsync(string connectionId, CancellationToken cancellationToken)
        {
            return await _context.ChatSessions.AsNoTracking().FirstOrDefaultAsync(x => x.ConnectionId == connectionId, cancellationToken);
        }
    }
}
