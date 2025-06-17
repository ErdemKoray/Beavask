using Beavask.Domain.Entities.Base;
using Beavask.Domain.Entities.Join;

namespace Beavask.Application.Interface.Repository;

public interface IProjectMemberRepository : IBaseRepository<ProjectMember, int>
{
    Task<List<ProjectMember>> GetAllUsersByProjectIdAsync(int projectId);
    Task<List<Project>> GetProjectsByUserIdAsync(int userId);
    Task<ProjectMember?> GetProjectMemberAsync(int userId, int projectId);
} 