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
        // DbContext üzerinden Projects tablosuna sorgu oluşturuyoruz
        var project = await dbContext.Projects
            .Where(p => p.RepoUrl == repoUrl && p.CompanyId == companyId && p.IsActive == true) // Repo URL, CompanyId ve IsActive true olmalı
            .FirstOrDefaultAsync(); // İlk eşleşen projeyi al

        // Eğer proje bulunursa ve tüm koşullar sağlanmışsa, true döner, aksi halde false
        return project != null; 
    }


    public async Task<bool> AskProjectNameExistsForUser(string repoUrl, int UserId)
    {
        // DbContext üzerinden Projects tablosuna sorgu oluşturuyoruz
        var projectExists = await dbContext.Projects
            .Where(p => p.RepoUrl == repoUrl && p.UserId == UserId && p.IsActive == true) // Repo URL, UserId ve IsActive true olmalı
            .AnyAsync(); // Sadece var olup olmadığını kontrol et

        return projectExists; // true/false döndür
    }

} 