using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class DependencyRepository : BaseRepository<Dependency, int>, IDependencyRepository
{
    public DependencyRepository(DbContext context) : base(context)
    {
    }
} 