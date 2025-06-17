using System.ComponentModel.DataAnnotations;

namespace Beavask.Application.DTOs.Friendship;

public class FriendshipRequest
{
    [Required]
    public required int ReceiverId { get; set; }
}
