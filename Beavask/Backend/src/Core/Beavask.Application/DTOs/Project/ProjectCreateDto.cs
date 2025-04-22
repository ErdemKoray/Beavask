namespace Beavask.Application.DTOs.Project;
public class ProjectCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CustomerId { get; set; }
}
