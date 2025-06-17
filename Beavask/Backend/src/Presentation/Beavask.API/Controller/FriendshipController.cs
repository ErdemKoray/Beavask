using Beavask.Application.Common;
using Beavask.Application.DTOs.Friendship;
using Beavask.Application.Interface.Service;
using Beavask.Domain.Entities.Join;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FriendshipController : ControllerBase
{
    private readonly IFriendshipService _friendshipService;
    private readonly IInvitationService _invitationService;

    public FriendshipController(IFriendshipService friendshipService, IInvitationService invitationService)
    {
        _friendshipService = friendshipService;
        _invitationService = invitationService;
    }

    [HttpGet("get-friends-list")]
    public async Task<IActionResult> GetFriendsList()
    {
        var result = await _friendshipService.GetFriendsListAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
}
