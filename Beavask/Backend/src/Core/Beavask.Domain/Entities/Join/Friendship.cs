using System;
using Beavask.Domain.Entities.Base;

namespace Beavask.Domain.Entities.Join;

public class Friendship
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;
    
    public required User Sender { get; set; }
    public int SenderId { get; set; }
    public required User Receiver { get; set; }
    public int ReceiverId { get; set; }
}
