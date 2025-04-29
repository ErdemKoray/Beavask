namespace Beavask.Application.Interface.Service
{
    public interface IMailService
    {
        Task SendTestEmailAsync();
        Task SendUserCredentialsAsync(string toEmail, string loginName, string password);
    }
}
