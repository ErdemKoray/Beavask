using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface IMessageRepository : IBaseRepository<Message, int>
{
    Task<IEnumerable<Message>> GetMessagesByFriendIdAsync(int currentUserId, int friendId);
} 