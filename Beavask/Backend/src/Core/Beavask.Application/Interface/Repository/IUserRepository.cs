using System.Linq.Expressions;
using Beavask.Application.Common;
using Beavask.Application.DTOs.User;
using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface IUserRepository : IBaseRepository<User, int>
{
    Task<UserWithTeamAndCompanyDto?> GetUserWithTeamAndCompanyBrief(Expression<Func<User, bool>> predicate, int userId);
    Task<IEnumerable<User>> GetWhereAsync(Expression<Func<User, bool>> predicate);
    Task<IEnumerable<User>> GetAllUsersByCompanyIdAsync(int companyId);
    Task<IEnumerable<User>> GetAllUsersByUserNameAsync(string userName);
    Task<User> IsUserAlreadyAssignedToCompany(string userName);

} 