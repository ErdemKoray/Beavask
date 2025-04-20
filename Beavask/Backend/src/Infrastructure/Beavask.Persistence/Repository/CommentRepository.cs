using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class CommentRepository : BaseRepository<Comment, int>, ICommentRepository
{
    public CommentRepository(DbContext context) : base(context)
    {
    }
} 