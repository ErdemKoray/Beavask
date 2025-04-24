using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface ITeamRepository : IBaseRepository<Team, int>
{
    Task<Team?> GetTeamWithMembersAsync(int teamId);
    Task<IEnumerable<User>> GetMembersByTeamId(int teamId);
} 