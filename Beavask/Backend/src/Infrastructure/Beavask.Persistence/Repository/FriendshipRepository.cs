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
}
