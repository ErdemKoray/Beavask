using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Join;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class RolePermissionRepository : BaseRepository<RolePermission, int>, IRolePermissionRepository
{
    public RolePermissionRepository(DbContext context) : base(context)
    {
    }
} 