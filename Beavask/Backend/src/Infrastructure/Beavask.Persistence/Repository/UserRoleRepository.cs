using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Join;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class UserRoleRepository : BaseRepository<UserRole, int>, IUserRoleRepository
{
    public UserRoleRepository(DbContext context) : base(context)
    {
    }
} 