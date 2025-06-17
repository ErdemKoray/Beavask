using Beavask.Application.DTOs.User;

namespace Beavask.Application.DTOs.Friendship;

public class PendingFriendRequestDto
{
    public int FriendshipId { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}