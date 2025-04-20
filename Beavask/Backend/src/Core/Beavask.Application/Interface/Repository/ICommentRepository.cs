using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface ICommentRepository : IBaseRepository<Comment, int>
{
    // Add any comment-specific repository methods here
} 