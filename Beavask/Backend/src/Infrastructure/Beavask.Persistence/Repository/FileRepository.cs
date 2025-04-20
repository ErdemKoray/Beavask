using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class FileRepository : BaseRepository<Domain.Entities.Base.File, int>, IFileRepository
{
    public FileRepository(DbContext context) : base(context)
    {
    }
} 