using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class UserContactRepository : BaseRepository<UserContact, int>, IUserContactRepository
{
    public UserContactRepository(DbContext context) : base(context)
    {
    }
} 