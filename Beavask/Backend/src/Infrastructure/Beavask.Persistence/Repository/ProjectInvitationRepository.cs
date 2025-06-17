using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Join;
using Beavask.Domain.Enums;
using Beavask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Beavask.Persistence.Repository;

public class ProjectInvitationRepository : BaseRepository<ProjectInvitation, int>, IProjectInvitationRepository
{
    private readonly BeavaskDbContext _dbContext;
    public ProjectInvitationRepository(BeavaskDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ProjectInvitation>> GetProjectInvitationsByUserIdAsync(int userId)
    {
        var projectInvitations = await _dbContext.ProjectInvitations
            .Where(pi => pi.ReceiverId == userId && pi.Status == ProjectInvitationStatus.Pending)
            .Include(pi => pi.Project)
            .Include(pi => pi.Sender)
            .ToListAsync();
        return projectInvitations;
    }
}
