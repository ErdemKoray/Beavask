using Beavask.Domain.Entities.Join;

namespace Beavask.Application.Interface.Repository;

public interface IProjectInvitationRepository : IBaseRepository<ProjectInvitation, int>
{
    Task<List<ProjectInvitation>> GetProjectInvitationsByUserIdAsync(int userId);
}
