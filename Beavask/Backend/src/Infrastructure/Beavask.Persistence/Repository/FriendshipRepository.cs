using Beavask.Application.DTOs.Friendship;
using Beavask.Application.DTOs.User;
using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Join;
using Beavask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Beavask.Persistence.Repository;

public class FriendshipRepository : BaseRepository<Friendship, int>, IFriendshipRepository
{
    private readonly BeavaskDbContext _context;
    public FriendshipRepository(BeavaskDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Friendship> GetFriendshipByIdAsync(int id)
    {
        return await _context.Friendships.Include(f => f.Sender).Include(f => f.Receiver).FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<bool> IsFriendshipExistsAsync(int senderId, int receiverId)
    {
        return await _context.Friendships.AnyAsync(f => 
            (f.SenderId == senderId && f.ReceiverId == receiverId) || 
            (f.SenderId == receiverId && f.ReceiverId == senderId));
    }

    public async Task<List<UserDto>> GetFriendsListAsync(int userId)
    {
        return await _context.Friendships
            .Include(f => f.Sender)
            .Include(f => f.Receiver)
            .Where(f => (f.SenderId == userId || f.ReceiverId == userId) && f.Status == FriendshipStatus.Accepted)
            .Select(f => new UserDto
            {
                Id = f.SenderId == userId ? f.Receiver.Id : f.Sender.Id,
                FirstName = f.SenderId == userId ? f.Receiver.FirstName : f.Sender.FirstName,
                LastName = f.SenderId == userId ? f.Receiver.LastName : f.Sender.LastName,
                Email = f.SenderId == userId ? f.Receiver.Email : f.Sender.Email,
                AvatarUrl = f.SenderId == userId ? f.Receiver.AvatarUrl : f.Sender.AvatarUrl,
                CreatedAt = f.SenderId == userId ? f.Receiver.CreatedAt : f.Sender.CreatedAt,
                UpdatedAt = f.SenderId == userId ? f.Receiver.UpdatedAt : f.Sender.UpdatedAt,
                IsActive = f.SenderId == userId ? f.Receiver.IsActive : f.Sender.IsActive,
                TeamId = f.SenderId == userId ? f.Receiver.TeamId ?? 0 : f.Sender.TeamId ?? 0,
                CompanyId = f.SenderId == userId ? f.Receiver.CompanyId ?? 0 : f.Sender.CompanyId ?? 0
            })
            .ToListAsync();
    }

    public async Task<List<PendingFriendRequestDto>> GetPendingFriendRequestsAsync(int userId)
    {
        return await _context.Friendships
            .Include(f => f.Sender)
            .Include(f => f.Receiver)
            .Where(f => f.ReceiverId == userId && f.Status == FriendshipStatus.Pending)
            .Select(f => new PendingFriendRequestDto
            {
                FriendshipId = f.Id,
                UserId = f.Sender.Id,
                Username = f.Sender.UserName,
                FirstName = f.Sender.FirstName,
                LastName = f.Sender.LastName
            })
            .ToListAsync();
    }
}
