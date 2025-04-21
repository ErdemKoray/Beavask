namespace Beavask.Application.DTOs.Problem;

public class ProblemCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
}
