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
    Task<Response<bool>> ChangeUserPasswordAsync(ChangeUserPasswordRequestDto dto);
    Task<Response<bool>> SendResetPasswordEmailAsync(string email);
    Task<Response<bool>> VerifyResetPasswordAsync(string email, string code);
    Task<Response<bool>> ForgotPasswordAsync(ForgotPasswordRequest dto);
    System.Threading.Tasks.Task AcceptInvitationAsync(string token, int userId);
    //GitHub
    Task<Response<string>> LoginWithGitHubAsync(GitHubLoginRequestDto dto, string clientId, string clientSecret);

}

