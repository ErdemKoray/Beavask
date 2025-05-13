using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Infrastructure.Persistence;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class LogRepository : BaseRepository<Log, int>, ILogRepository
{
    public LogRepository(BeavaskDbContext context) : base(context)
    {
    }
} 