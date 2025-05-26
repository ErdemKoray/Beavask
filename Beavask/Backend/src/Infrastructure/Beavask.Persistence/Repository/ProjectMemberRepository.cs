using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Domain.Entities.Join;
using Beavask.Infrastructure.Persistence;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class ProjectMemberRepository : BaseRepository<ProjectMember, int>, IProjectMemberRepository
{
    private readonly BeavaskDbContext _context;
    public ProjectMemberRepository(BeavaskDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<ProjectMember>> GetAllUsersByProjectIdAsync(int projectId)
    {
        var projectMembers = await _context.ProjectMembers
            .Where(pm => pm.ProjectId == projectId)
            .ToListAsync();
        return projectMembers;
    }
    public async Task<List<Project>> GetProjectsByUserIdAsync(int userId)
    {
        var projects = await _context.ProjectMembers
            .Where(pm => pm.UserId == userId && pm.IsActive)
            .Include(pm => pm.Project)  // Proje bilgisi ile birlikte al
            .Select(pm => pm.Project)
            .Where(p => p != null && p.IsActive)
            .ToListAsync();

        return projects;
    }



} 