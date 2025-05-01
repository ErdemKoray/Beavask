using System.Linq.Expressions;
using Beavask.Application.DTOs.User;
using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface IUserRepository : IBaseRepository<User, int>
{
    Task<UserWithTeamAndCompanyDto?> GetUserWithTeamAndCompanyBrief(Expression<Func<User, bool>> predicate, int userId);

} 