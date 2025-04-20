using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface ILogRepository : IBaseRepository<Log, int>
{
    // Add any log-specific repository methods here
} 