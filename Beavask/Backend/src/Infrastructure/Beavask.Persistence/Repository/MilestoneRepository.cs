using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class MilestoneRepository : BaseRepository<Milestone, int>, IMilestoneRepository
{
    public MilestoneRepository(DbContext context) : base(context)
    {
    }
} 