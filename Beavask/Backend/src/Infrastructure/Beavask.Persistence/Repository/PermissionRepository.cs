using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class PermissionRepository : BaseRepository<Permission, int>, IPermissionRepository
{
    public PermissionRepository(DbContext context) : base(context)
    {
    }
} 