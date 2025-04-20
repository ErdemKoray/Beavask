using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class LogRepository : BaseRepository<Log, int>, ILogRepository
{
    public LogRepository(DbContext context) : base(context)
    {
    }
} 