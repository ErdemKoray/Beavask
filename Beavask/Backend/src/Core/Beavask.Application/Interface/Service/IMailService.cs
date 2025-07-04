using Beavask.Application.DTOs.Auth;

namespace Beavask.Application.Interface.Service
{
    public interface IMailService
    {
        Task SendVerificationCodeAsync(string toEmail, string verificationCode);
        Task SendUserCredentialsAsync(string toEmail, string loginName, string password);
        Task SendIndividualVerificationCodeAsync(string toEmail, string verificationCode);
        Task SendRegistrationSuccessEmailAsync(string toEmail);
        Task SendProjectInvitationAsync(ProjectInvitationRequest request);
        Task SendForgotPasswordEmailAsync(string toEmail, string verificationCode);
    }
}
