using System.Linq.Expressions;
using Beavask.Application.DTOs.User;
using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Infrastructure.Persistence;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class UserRepository : BaseRepository<User, int>, IUserRepository
{
    private readonly BeavaskDbContext _context;
    public UserRepository(BeavaskDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserWithTeamAndCompanyDto?> GetUserWithTeamAndCompanyBrief(Expression<Func<User, bool>> predicate, int userId)
    {
        var query = from user in _context.Users
                    join team in _context.Teams on user.TeamId equals team.Id into teamGroup
                    from team in teamGroup.DefaultIfEmpty()

                    join company in _context.Companies on user.CompanyId equals company.Id into companyGroup
                    from company in companyGroup.DefaultIfEmpty()

                    join userContact in _context.UserContacts
                        .Where(c => c.ContactType == "PhoneNumber")
                        on user.Id equals userContact.UserId into userContactGroup
                    from userContact in userContactGroup.DefaultIfEmpty()

                    where user.Id == userId
                    select new UserWithTeamAndCompanyDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = userContact != null ? userContact.ContactValue : null,
                        TeamId = user.TeamId,
                        CompanyId = user.CompanyId,
                        TeamName = team != null ? team.Title : null,
                        CompanyName = company != null ? company.Name : null
                    };

    return await query.FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<User>> GetWhereAsync(Expression<Func<User, bool>> predicate)
    {
        return await _context.Users.Where(predicate).ToListAsync();
    }
    public async Task<IEnumerable<User>> GetAllUsersByCompanyIdAsync(int companyId)
    {
        return await _context.Users
            .Where(u => u.CompanyId == companyId && u.IsActive == true)
            .ToListAsync();
    }
    public async Task<IEnumerable<User>> GetAllUsersByUserNameAsync(string userName)
    {
        return await _context.Users
            .Where(u => u.UserName.Contains(userName))
            .ToListAsync();
    }

    public async Task<User> IsUserAlreadyAssignedToCompany(string userName)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName && u.CompanyId != null);
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == username);
    }
} 