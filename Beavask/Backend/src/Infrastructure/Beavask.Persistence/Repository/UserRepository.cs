using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class UserRepository : BaseRepository<User, int>, IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
    }
} 