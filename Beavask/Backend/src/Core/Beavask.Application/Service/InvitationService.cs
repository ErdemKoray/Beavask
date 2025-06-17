using Beavask.Application.Common;
using Beavask.Application.DTOs.Friendship;
using Beavask.Application.DTOs.NotificationDtos;
using Beavask.Application.Interface;
using Beavask.Application.Interface.Repository;
using Beavask.Application.Interface.Service;
using Beavask.Domain.Entities.Base;
using Beavask.Domain.Entities.Join;

namespace Beavask.Application.Service;

public class InvitationService : IInvitationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;

    public InvitationService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _notificationService = notificationService;
    }
    public async Task<Response<bool>> SendFriendshipRequest(FriendshipRequest request)
    {
        try
        {
            var senderId = _currentUserService.UserId.Value;
            var sender = await _unitOfWork.UserRepository.GetByIdAsync(senderId);
            if (sender == null)
            {
                return Response<bool>.Fail("Sender not found");
            }
            var receiver = await _unitOfWork.UserRepository.GetByIdAsync(request.ReceiverId);
            if (receiver == null)
            {
                return Response<bool>.Fail("Receiver not found");
            }
            var friendship = new Friendship
            {
                SenderId = senderId,
                ReceiverId = request.ReceiverId,
                Sender = sender,
                Receiver = receiver,
                Status = FriendshipStatus.Pending
            };
            await _unitOfWork.FriendshipRepository.AddAsync(friendship);

            var notificationDto = new NotificationCreateDto
            {
                NotificationType = "FriendshipRequest",
                Title = "Friendship Request",
                Content = $"You have a new friendship request from {sender.FirstName} {sender.LastName}",
                UserId = receiver.Id
            };
            await _notificationService.CreateAsync(notificationDto);
            await _unitOfWork.SaveChangesAsync();
            return Response<bool>.Success(true, "Friendship request sent successfully");
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> AcceptFriendshipRequest(int friendshipId)
    {
        try
        {
            var friendship = await _unitOfWork.FriendshipRepository.GetFriendshipByIdAsync(friendshipId);
            if (friendship == null)
            {
                return Response<bool>.Fail("Friendship not found");
            }
            friendship.Status = FriendshipStatus.Accepted;
            await _unitOfWork.FriendshipRepository.UpdateAsync(friendship);
            var notificationDto = new NotificationCreateDto
            {
                NotificationType = "FriendshipRequestAccepted",
                Title = "Friendship Request Accepted",
                Content = $"{friendship.Sender.FirstName} {friendship.Sender.LastName} has accepted your friendship request",
                UserId = friendship.SenderId
            };
            await _notificationService.CreateAsync(notificationDto);
            await _unitOfWork.SaveChangesAsync();
            return Response<bool>.Success(true, "Friendship request accepted successfully");
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> RejectFriendshipRequest(int friendshipId)
    {
        try
        {
            var friendship = await _unitOfWork.FriendshipRepository.GetFriendshipByIdAsync(friendshipId);
            if (friendship == null)
            {
                return Response<bool>.Fail("Friendship not found");
            }
            friendship.Status = FriendshipStatus.Rejected;
            await _unitOfWork.FriendshipRepository.UpdateAsync(friendship);
            var notificationDto = new NotificationCreateDto
            {
                NotificationType = "FriendshipRequestRejected",
                Title = "Friendship Request Rejected",
                Content = $"{friendship.Sender.FirstName} {friendship.Sender.LastName} has rejected your friendship request",
                UserId = friendship.SenderId
            };
            await _notificationService.CreateAsync(notificationDto);
            await _unitOfWork.SaveChangesAsync();
            return Response<bool>.Success(true, "Friendship request rejected successfully");
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }
}

