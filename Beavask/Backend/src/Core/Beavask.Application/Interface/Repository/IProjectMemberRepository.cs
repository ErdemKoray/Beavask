using Beavask.Domain.Entities.Join;

namespace Beavask.Application.Interface.Repository;

public interface IProjectMemberRepository : IBaseRepository<ProjectMember, int>
{
    Task<List<ProjectMember>> GetAllUsersByProjectIdAsync(int projectId);
} 