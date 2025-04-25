using Beavask.Application.Common;
using Beavask.Application.DTOs.Auth;

namespace Beavask.Application.Interface.Service;

public interface IAuthService
{
    Task<Response<string>> LoginAsync(LoginRequestDto dto);
    Task<Response<bool>> RegisterAsync(RegisterRequestDto dto);

    //GitHub
    Task<Response<string>> LoginWithGitHubAsync(GitHubLoginRequestDto dto, string clientId, string clientSecret);

}

