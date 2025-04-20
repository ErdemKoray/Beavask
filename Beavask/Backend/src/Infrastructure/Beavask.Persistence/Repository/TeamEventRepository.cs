using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Join;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class TeamEventRepository : BaseRepository<TeamEvent, int>, ITeamEventRepository
{
    public TeamEventRepository(DbContext context) : base(context)
    {
    }
} 