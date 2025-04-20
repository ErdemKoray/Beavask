using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class TeamRepository : BaseRepository<Team, int>, ITeamRepository
{
    public TeamRepository(DbContext context) : base(context)
    {
    }
} 