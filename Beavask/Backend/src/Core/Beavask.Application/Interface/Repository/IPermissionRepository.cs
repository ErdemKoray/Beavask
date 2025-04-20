using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface IPermissionRepository : IBaseRepository<Permission, int>
{
    // Add any permission-specific repository methods here
} 