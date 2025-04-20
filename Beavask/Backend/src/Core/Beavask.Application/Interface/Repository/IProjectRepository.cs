using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface IProjectRepository : IBaseRepository<Project, int>
{
    // Add any project-specific repository methods here
} 