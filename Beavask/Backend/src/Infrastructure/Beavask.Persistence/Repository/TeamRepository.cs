using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class TeamRepository : BaseRepository<Team, int>, ITeamRepository
{
    private readonly BeavaskDbContext _context;

    public TeamRepository(BeavaskDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Team?> GetTeamWithMembersAsync(int teamId)
    {
        return await _dbSet
            .Include(t => t.TeamMembers)
            .FirstOrDefaultAsync(t => t.Id == teamId);
    }

    public async Task<IEnumerable<User>> GetMembersByTeamId(int teamId)
    {
        return await _context.Users
            .Where(u => u.TeamId == teamId)
            .ToListAsync();
    }

    public Task<List<Event>> GetEventsByTeamId(int teamId)
    {
        return _context.Events
            .Where(e => e.Teams.Any(te => te.TeamId == teamId))
            .ToListAsync();
    }
}
