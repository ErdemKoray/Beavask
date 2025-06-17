using Beavask.Application.Common;
using Beavask.Application.DTOs.Friendship;
using Beavask.Application.DTOs.User;
using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Service;

public interface IInvitationService
{
    Task<Response<bool>> SendFriendshipRequest(FriendshipRequest request);
    Task<Response<bool>> AcceptFriendshipRequest(int friendshipId);
    Task<Response<bool>> RejectFriendshipRequest(int friendshipId);
    Task<Response<List<PendingFriendRequestDto>>> GetPendingFriendRequestsAsync();
    Task<Response<bool>> InviteFriendToPersonelProject(PersonelProjectInvitationRequest request);
    Task<Response<bool>> AcceptProjectInvitation(int invitationId);
    Task<Response<bool>> RejectProjectInvitation(int invitationId);
}
