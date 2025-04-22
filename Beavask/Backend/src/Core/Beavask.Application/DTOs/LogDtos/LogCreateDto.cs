namespace Beavask.Application.DTOs.LogDtos;

public class LogCreateDto
{
    public string ActivityType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
}
