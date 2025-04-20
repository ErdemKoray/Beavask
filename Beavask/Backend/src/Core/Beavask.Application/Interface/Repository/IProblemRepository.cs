using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface IProblemRepository : IBaseRepository<Problem, int>
{
    // Add any problem-specific repository methods here
} 