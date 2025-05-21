using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Join;
using Beavask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class UserRoleRepository : BaseRepository<UserRole, int>, IUserRoleRepository
{
    private readonly BeavaskDbContext _context;
    public UserRoleRepository(BeavaskDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserRole> AssignRoleToUserAsync(int userId, int roleId)
    {
        var user = await _context.Users.FindAsync(userId);
        var role = await _context.Roles.FindAsync(roleId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (role == null)
        {
            throw new Exception("Role not found");
        }

        var userRole = new UserRole
        {
            UserId = userId,
            RoleId = roleId,
            User = user,
            Role = role
        };

        await AddAsync(userRole);

        return userRole;
    }
} 