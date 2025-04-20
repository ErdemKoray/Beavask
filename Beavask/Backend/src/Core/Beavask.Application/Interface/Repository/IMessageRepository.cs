using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface IMessageRepository : IBaseRepository<Message, int>
{
    // Add any message-specific repository methods here
} 