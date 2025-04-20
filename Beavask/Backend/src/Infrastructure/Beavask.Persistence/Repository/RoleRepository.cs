using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class RoleRepository : BaseRepository<Role, int>, IRoleRepository
{
    public RoleRepository(DbContext context) : base(context)
    {
    }
} 