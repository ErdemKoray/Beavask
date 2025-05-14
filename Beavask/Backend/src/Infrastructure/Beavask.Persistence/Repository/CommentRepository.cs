using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Infrastructure.Persistence;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class CommentRepository : BaseRepository<Comment, int>, ICommentRepository
{
    private readonly BeavaskDbContext _context;
    public CommentRepository(BeavaskDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Comment>> GetAllByUserIdAsync(int userId)
    {
        return await _context.Comments
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

} 