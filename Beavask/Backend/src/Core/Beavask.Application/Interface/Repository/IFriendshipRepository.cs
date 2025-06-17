using Beavask.Domain.Entities.Join;

namespace Beavask.Application.Interface.Repository
{
    public interface IFriendshipRepository : IBaseRepository<Friendship, int>
    {
        Task<Friendship> GetFriendshipByIdAsync(int id);
        Task<bool> IsFriendshipExistsAsync(int senderId, int receiverId);
    }
}


