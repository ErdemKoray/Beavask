using Beavask.Application.Common;
using Beavask.Application.DTOs.Auth;
using Beavask.Application.DTOs.Company;

namespace Beavask.Application.Interface.Service;

public interface IAuthService
{
    Task<Response<string>> LoginAsync(LoginRequestDto dto);
    Task<Response<string>> LoginCompanyAsync(CompanyLoginRequestDto dto);
    Task<Response<bool>> RegisterAsync(RegisterRequestDto dto);
    Task<Response<bool>> RegisterCompanyAsync(CompanyCreateDto dto);
    Task<Response<bool>> VerifyCompanyEmailAsync(string email, string code);
    Task<Response<bool>> VerifyPersonelEmailAsync(string email, string code);
    Task<Response<bool>> ChangeCompanyPasswordAsync(ChangeCompanyPasswordRequestDto dto);

    //GitHub
    Task<Response<string>> LoginWithGitHubAsync(GitHubLoginRequestDto dto, string clientId, string clientSecret);

}

