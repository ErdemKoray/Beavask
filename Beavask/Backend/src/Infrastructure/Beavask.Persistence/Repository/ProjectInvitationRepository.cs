using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Join;
using Beavask.Infrastructure.Persistence;


namespace Beavask.Persistence.Repository;

public class ProjectInvitationRepository : BaseRepository<ProjectInvitation, int>, IProjectInvitationRepository
{
    public ProjectInvitationRepository(BeavaskDbContext context) : base(context)
    {
    }
}
