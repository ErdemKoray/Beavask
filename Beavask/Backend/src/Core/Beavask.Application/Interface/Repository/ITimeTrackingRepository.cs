using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface ITimeTrackingRepository : IBaseRepository<TimeTracking, int>
{
    // Add any time tracking-specific repository methods here
} 