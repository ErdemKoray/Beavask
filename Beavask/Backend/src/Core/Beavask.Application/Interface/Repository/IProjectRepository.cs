using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface IProjectRepository : IBaseRepository<Project, int>
{
    Task<bool> AskProjectNameExistsForUser(string repoUrl, int UserId);
    Task<bool> AskProjectNameExistsForCompany(string repoUrl, int companyId);
    Task<List<Project>> GetAllProjectsByCompanyId(int companyId);
    Task<List<Project>> GetAllProjectsByUserId(int userId);
} 