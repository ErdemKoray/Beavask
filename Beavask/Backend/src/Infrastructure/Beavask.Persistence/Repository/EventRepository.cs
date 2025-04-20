using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class EventRepository : BaseRepository<Event, int>, IEventRepository
{
    public EventRepository(DbContext context) : base(context)
    {
    }
} 