using Beavask.Application.DTOs.Auth;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("send-project-invitation")]
        public async Task<IActionResult> SendProjectInvitation([FromBody] ProjectInvitationRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.ToEmail))
                return BadRequest("Recipient email is required.");

            try
            {
                await _mailService.SendProjectInvitationAsync(request);
                return Ok(new { message = "Invitation email sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while sending the email.", detail = ex.Message });
            }
        }
    }
}
