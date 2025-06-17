using Beavask.Application.Common;
using Beavask.Application.DTOs.Friendship;
using Beavask.Application.DTOs.User;
using Beavask.Application.Interface;
using Beavask.Application.Interface.Repository;
using Beavask.Application.Interface.Service;
using Beavask.Domain.Entities.Join;

namespace Beavask.Application.Service;

public class FriendshipService : IFriendshipService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly ICurrentUserService _currentUserService;

    public FriendshipService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _friendshipRepository = unitOfWork.FriendshipRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Response<List<UserDto>>> GetFriendsListAsync()
    {
        try
        {
            var userId = _currentUserService.UserId.Value;
            var friendships = await _friendshipRepository.GetFriendsListAsync(userId);
            return Response<List<UserDto>>.Success(friendships, "Friendships fetched successfully");
        }
        catch (Exception ex)
        {
            return Response<List<UserDto>>.Fail(ex.Message);
        }
    }
}
