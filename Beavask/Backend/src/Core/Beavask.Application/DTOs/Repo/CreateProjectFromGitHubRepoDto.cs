namespace Beavask.Application.DTOs.Repo;
public class CreateProjectFromGitHubRepoDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? RepoUrl { get; set; }
    public bool IsCompanyProject { get; set; } = false;
}
