namespace Beavask.Application.DTOs.Problem;

public class ProblemUpdateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
