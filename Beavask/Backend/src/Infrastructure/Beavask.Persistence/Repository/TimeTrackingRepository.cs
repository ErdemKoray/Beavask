using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class TimeTrackingRepository : BaseRepository<TimeTracking, int>, ITimeTrackingRepository
{
    public TimeTrackingRepository(DbContext context) : base(context)
    {
    }
} 