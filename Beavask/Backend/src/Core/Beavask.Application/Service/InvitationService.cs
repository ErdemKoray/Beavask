using Beavask.Application.Common;
using Beavask.Application.DTOs.Auth;
using Beavask.Application.DTOs.Friendship;
using Beavask.Application.DTOs.Invitation;
using Beavask.Application.DTOs.NotificationDtos;
using Beavask.Application.DTOs.User;
using Beavask.Application.Interface;
using Beavask.Application.Interface.Repository;
using Beavask.Application.Interface.Service;
using Beavask.Domain.Entities.Base;
using Beavask.Domain.Entities.Join;
using Beavask.Domain.Enums;

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
            if (await _unitOfWork.FriendshipRepository.IsFriendshipExistsAsync(senderId, request.ReceiverId))
            {
                return Response<bool>.Fail("Friendship already exists");
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

    public async Task<Response<List<PendingFriendRequestDto>>> GetPendingFriendRequestsAsync()
    {
        try
        {
            var userId = _currentUserService.UserId.Value;
            var pendingFriendRequests = await _unitOfWork.FriendshipRepository.GetPendingFriendRequestsAsync(userId);
            return Response<List<PendingFriendRequestDto>>.Success(pendingFriendRequests, "Pending friend requests fetched successfully");
        }
        catch (Exception ex)
        {
            return Response<List<PendingFriendRequestDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> InviteFriendToPersonelProject(PersonelProjectInvitationRequest request)
    {
        try
        {
            if (request == null)
            {
                return Response<bool>.Fail("Request object is null");
            }

            if (!_currentUserService.UserId.HasValue)
            {
                return Response<bool>.Fail("Current user ID is not available");
            }

            var senderId = _currentUserService.UserId.Value;
            var sender = await _unitOfWork.UserRepository.GetByIdAsync(senderId);
            if (sender == null)
            {
                return Response<bool>.Fail($"Sender not found with ID: {senderId}");
            }

            if (request.UserId <= 0)
            {
                return Response<bool>.Fail("Invalid receiver user ID");
            }

            var receiver = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
            if (receiver == null)
            {
                return Response<bool>.Fail($"Receiver not found with ID: {request.UserId}");
            }

            if (request.ProjectId <= 0)
            {
                return Response<bool>.Fail("Invalid project ID");
            }

            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(request.ProjectId);
            if (project == null)
            {
                return Response<bool>.Fail($"Project not found with ID: {request.ProjectId}");
            }

            var invitation = new ProjectInvitation
            {
                SenderId = senderId,
                ReceiverId = request.UserId,
                ProjectId = request.ProjectId,
                Sender = sender,
                Receiver = receiver,
                Project = project,
                Status = ProjectInvitationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ProjectInvitationRepository.AddAsync(invitation);

            var notificationDto = new NotificationCreateDto
            {
                NotificationType = "ProjectInvitation",
                Title = "Project Invitation",
                Content = $"{sender.UserName} has invited you to join their personal project named {project.Name}",
                UserId = receiver.Id
            };

            await _notificationService.CreateAsync(notificationDto);
            await _unitOfWork.SaveChangesAsync();
            return Response<bool>.Success(true, "Project invitation sent successfully");
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail($"An error occurred while sending project invitation: {ex.Message}");
        }
    }

    public async Task<Response<bool>> AcceptProjectInvitation(ProjectInvitationIdRequest request)
    {
        try
        {
            if (request.ProjectInvitationId <= 0)
            {
                return Response<bool>.Fail("Invalid invitation ID");
            }

            var invitation = await _unitOfWork.ProjectInvitationRepository.GetByIdAsync(request.ProjectInvitationId);
            if (invitation == null)
            {
                return Response<bool>.Fail($"Invitation not found with ID: {request.ProjectInvitationId}");
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(invitation.ReceiverId);
            if (user == null)
            {
                return Response<bool>.Fail($"User not found with ID: {invitation.ReceiverId}");
            }

            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(invitation.ProjectId);
            if (project == null)
            {
                return Response<bool>.Fail($"Project not found with ID: {invitation.ProjectId}");
            }

            // Check if user is already a member of the project
            var existingMembership = await _unitOfWork.ProjectMemberRepository.GetProjectMemberAsync(user.Id, project.Id);
            if (existingMembership != null)
            {
                return Response<bool>.Fail("User is already a member of this project");
            }

            invitation.Status = ProjectInvitationStatus.Accepted;
            await _unitOfWork.ProjectInvitationRepository.UpdateAsync(invitation);

            var projectMember = new ProjectMember
            {
                UserId = user.Id,
                ProjectId = project.Id,
                User = user,
                Project = project,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ProjectMemberRepository.AddAsync(projectMember);

            var notificationDto = new NotificationCreateDto
            {
                NotificationType = "ProjectInvitationAccepted",
                Title = "Project Invitation Accepted",
                Content = $"{user.UserName} has accepted your invitation to join the project {project.Name}",
                UserId = invitation.SenderId
            };

            await _notificationService.CreateAsync(notificationDto);
            await _unitOfWork.SaveChangesAsync();

            return Response<bool>.Success(true, "Project invitation accepted successfully");
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail($"An error occurred while accepting project invitation: {ex.Message}");
        }
    }

    public async Task<Response<bool>> RejectProjectInvitation(ProjectInvitationIdRequest request)
    {
        try
        {
            var invitation = await _unitOfWork.ProjectInvitationRepository.GetByIdAsync(request.ProjectInvitationId);
            if (invitation == null)
            {
                return Response<bool>.Fail("Project invitation not found");
            }

            var currentUserId = _currentUserService.UserId.Value;
            if (invitation.ReceiverId != currentUserId)
            {
                return Response<bool>.Fail("You are not authorized to reject this invitation");
            }

            invitation.Status = ProjectInvitationStatus.Rejected;
            await _unitOfWork.ProjectInvitationRepository.UpdateAsync(invitation);

            var notificationDto = new NotificationCreateDto
            {
                NotificationType = "ProjectInvitationRejected",
                Title = "Project Invitation Rejected",
                Content = $"Your project invitation has been rejected",
                UserId = invitation.SenderId
            };

            await _notificationService.CreateAsync(notificationDto);
            await _unitOfWork.SaveChangesAsync();

            return Response<bool>.Success(true, "Project invitation rejected successfully");
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail($"An error occurred while rejecting project invitation: {ex.Message}");
        }
    }

    public async Task<Response<List<ProjectInvitationDto>>> GetProjectInvitationsAsync()
    {
        try
        {
            if (!_currentUserService.UserId.HasValue)
            {
                return Response<List<ProjectInvitationDto>>.Fail("User ID is not available");
            }

            var userId = _currentUserService.UserId.Value;
            var projectInvitations = await _unitOfWork.ProjectInvitationRepository.GetProjectInvitationsByUserIdAsync(userId);
            
            if (projectInvitations == null)
            {
                return Response<List<ProjectInvitationDto>>.Success(new List<ProjectInvitationDto>(), "No project invitations found");
            }

            var projectInvitationDtos = projectInvitations.Select(invitation => new ProjectInvitationDto
            {
                Id = invitation.Id,
                ProjectId = invitation.ProjectId,
                SenderId = invitation.SenderId,
                Status = invitation.Status,
                CreatedAt = invitation.CreatedAt,
                SenderName = invitation.Sender?.UserName ?? "Unknown User",
                ProjectName = invitation.Project?.Name ?? "Unknown Project"
            }).ToList();

            return Response<List<ProjectInvitationDto>>.Success(projectInvitationDtos, "Project invitations fetched successfully");
        }
        catch (Exception ex)
        {
            return Response<List<ProjectInvitationDto>>.Fail($"An error occurred while fetching project invitations: {ex.Message}");
        }
    }
}

