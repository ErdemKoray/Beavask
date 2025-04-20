using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface IUserRepository : IBaseRepository<User, int>
{
    // Add any user-specific repository methods here
} 