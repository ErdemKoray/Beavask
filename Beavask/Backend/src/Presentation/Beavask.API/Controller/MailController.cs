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

        [HttpPost("send-test")]
        public async Task<IActionResult> SendTestEmail()
        {
            await _mailService.SendTestEmailAsync();
            return Ok("Test email has been sent.");
        }
    }
}
