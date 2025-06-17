using Beavask.Application.Interface.Repository;
using Beavask.Application.Interface.Service;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class MessageRepository : BaseRepository<Message, int>, IMessageRepository
{

    public MessageRepository(DbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Message>> GetMessagesByFriendIdAsync(int currentUserId, int friendId)
    {
        return await _context.Set<Message>()
            .Where(m => (m.SenderId == currentUserId && m.ReceiverId == friendId) || 
                       (m.SenderId == friendId && m.ReceiverId == currentUserId))
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }
} 