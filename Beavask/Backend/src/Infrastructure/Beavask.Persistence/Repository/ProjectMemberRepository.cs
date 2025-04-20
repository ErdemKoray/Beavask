using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Join;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class ProjectMemberRepository : BaseRepository<ProjectMember, int>, IProjectMemberRepository
{
    public ProjectMemberRepository(DbContext context) : base(context)
    {
    }
} 