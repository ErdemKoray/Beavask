using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface ITeamRepository : IBaseRepository<Team, int>
{
    // Add any team-specific repository methods here
} 