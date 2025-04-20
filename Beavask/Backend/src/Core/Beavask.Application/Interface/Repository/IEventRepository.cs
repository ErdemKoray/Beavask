using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface IEventRepository : IBaseRepository<Event, int>
{
    // Add any event-specific repository methods here
} 