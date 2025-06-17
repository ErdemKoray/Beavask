using Beavask.Application.Common;
using Beavask.Application.DTOs.Friendship;
using Beavask.Application.DTOs.User;
using Beavask.Domain.Entities.Join;

namespace Beavask.Application.Interface.Service;

public interface IFriendshipService
{
    Task<Response<List<UserDto>>> GetFriendsListAsync();
}

