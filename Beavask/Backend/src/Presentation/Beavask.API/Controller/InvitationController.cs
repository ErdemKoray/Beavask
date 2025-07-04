using Beavask.Application.Common;
using Beavask.Application.DTOs.Auth;
using Beavask.Application.DTOs.Friendship;
using Beavask.Application.DTOs.Invitation;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvitationController : ControllerBase
{
    private readonly IInvitationService _invitationService;

    public InvitationController(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    [HttpPost("send-friendship-request")]
    public async Task<IActionResult> SendFriendshipRequest([FromBody] FriendshipRequest request)
    {
        var result = await _invitationService.SendFriendshipRequest(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPost("accept-friendship-request")]
    public async Task<IActionResult> AcceptFriendshipRequest([FromBody] FriendReqDto friendReqDto)
    {
        var result = await _invitationService.AcceptFriendshipRequest(friendReqDto.FriendshipId);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPost("reject-friendship-request")]
    public async Task<IActionResult> RejectFriendshipRequest([FromBody] FriendReqDto friendReqDto)
    {
        var result = await _invitationService.RejectFriendshipRequest(friendReqDto.FriendshipId);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpGet("get-pending-friend-requests")]
    public async Task<IActionResult> GetPendingFriendRequests()
    {
        var result = await _invitationService.GetPendingFriendRequestsAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPost("invite-friend-to-personal-project")]
    public async Task<IActionResult> InviteFriendToPersonalProject([FromBody] PersonelProjectInvitationRequest request)
    {
        var result = await _invitationService.InviteFriendToPersonelProject(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPost("accept-project-invitation")]
    public async Task<IActionResult> AcceptProjectInvitation([FromBody] ProjectInvitationIdRequest request)
    {
        var result = await _invitationService.AcceptProjectInvitation(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPost("reject-project-invitation")]
    public async Task<IActionResult> RejectProjectInvitation([FromBody] ProjectInvitationIdRequest request)
    {
        var result = await _invitationService.RejectProjectInvitation(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpGet("get-project-invitations")]
    public async Task<IActionResult> GetProjectInvitations()
    {
        var result = await _invitationService.GetProjectInvitationsAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
}
