using Beavask.Domain.Entities.Join;

namespace Beavask.Application.Interface.Repository;

public interface IUserRoleRepository : IBaseRepository<UserRole, int>
{
    Task<UserRole> AssignRoleToUserAsync(int userId, int roleId);
} 