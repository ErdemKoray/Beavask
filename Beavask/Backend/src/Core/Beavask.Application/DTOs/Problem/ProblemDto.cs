namespace Beavask.Application.DTOs.Problem;

public class ProblemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int UserId { get; set; }
}