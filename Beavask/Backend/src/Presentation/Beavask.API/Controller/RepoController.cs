using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RepoController : ControllerBase
    {
        private readonly IRepoService _repoService;

        public RepoController(IRepoService repoService)
        {
            _repoService = repoService;
        }

        [HttpGet("my-public-repos")]
        public async Task<IActionResult> GetMyPublicRepositories()
        {
            var result = await _repoService.GetCurrentUserPublicRepositoriesAsync();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
