using ChatAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatAssistant.Infrastructure.Data.Repositories.Teams
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ChatAssistantDbContext _context;

        public TeamRepository(ChatAssistantDbContext context)
        {
            _context = context;
        }

        public async Task<Team> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
          
            return await _context.Teams
                .Include(t => t.Agents)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Teams
                .Include(t => t.Agents) 
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Team team, CancellationToken cancellationToken)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Team team, CancellationToken cancellationToken)
        {
            _context.Entry(team).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Team team, CancellationToken cancellationToken)
        {
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
