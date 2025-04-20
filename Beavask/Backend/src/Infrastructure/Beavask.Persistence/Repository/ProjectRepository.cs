using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class ProjectRepository : BaseRepository<Project, int>, IProjectRepository
{
    public ProjectRepository(DbContext context) : base(context)
    {
    }
} 