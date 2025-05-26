using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Infrastructure.Persistence;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class ProjectRepository : BaseRepository<Project, int>, IProjectRepository
{
    private readonly BeavaskDbContext dbContext;
    public ProjectRepository(BeavaskDbContext context) : base(context)
    {
        dbContext = context;
    }

    public async Task<bool> AskProjectNameExistsForCompany(string repoUrl, int companyId)
    {
        var project = await dbContext.Projects
            .Where(p => p.RepoUrl == repoUrl && p.CompanyId == companyId && p.IsActive == true)
            .FirstOrDefaultAsync();
        return project != null; 
    }

    public async Task<bool> AskProjectNameExistsForUser(string repoUrl, int UserId)
    {
        var projectExists = await dbContext.Projects
            .Where(p => p.RepoUrl == repoUrl && p.UserId == UserId && p.IsActive == true)
            .AnyAsync();
        return projectExists;
    }

    public async Task<List<Project>> GetAllProjectsByCompanyId(int companyId)
    {
        var projects = await dbContext.Projects
            .Where(p => p.CompanyId == companyId)
            .ToListAsync();
        return projects;
    }

    public Task<List<Project>> GetAllProjectsByUserId(int userId)
    {
        var projects = dbContext.Projects
            .Where(p => p.UserId == userId)
            .ToListAsync();
        return projects;
    }
} 