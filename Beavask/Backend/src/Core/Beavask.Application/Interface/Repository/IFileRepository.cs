using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface IFileRepository : IBaseRepository<Domain.Entities.Base.File, int>
{
    // Add any file-specific repository methods here
} 