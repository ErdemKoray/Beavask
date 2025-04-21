namespace Beavask.Application.DTOs.LogDtos;

public class LogDto
{
    public int Id { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
}
