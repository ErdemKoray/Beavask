using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface ICommentRepository : IBaseRepository<Comment, int>
{
    Task<IEnumerable<Comment>> GetAllByUserIdAsync(int userId);
} 