using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface IDependencyRepository : IBaseRepository<Dependency, int>
{
    // Add any dependency-specific repository methods here
} 