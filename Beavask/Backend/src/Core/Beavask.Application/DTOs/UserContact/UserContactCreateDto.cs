using System.ComponentModel.DataAnnotations;

namespace Beavask.Application.DTOs.UserContact;

public class UserContactCreateDto
{
    [Required]
    public string ContactType { get; set; } = string.Empty;

    [Required]
    public string ContactValue { get; set; } = string.Empty;

    [Required]
    public int UserId { get; set; }
}
