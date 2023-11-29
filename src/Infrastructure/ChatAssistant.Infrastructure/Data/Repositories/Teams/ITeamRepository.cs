using ChatAssistant.Domain.Entities;

namespace ChatAssistant.Infrastructure.Data.Repositories.Teams
{
    public interface ITeamRepository
    {
        Task<Team> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Team team, CancellationToken cancellationToken);
        Task UpdateAsync(Team team, CancellationToken cancellationToken);
        Task DeleteAsync(Team team, CancellationToken cancellationToken);
    }
}
