using Beavask.Application.Interface.Service;
using System.Net;
using System.Net.Mail;

namespace Beavask.API.Service
{
    public class GmailMailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public GmailMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendVerificationCodeAsync(string toEmail, string verificationCode)     
       {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Mail:Username"]),
                Subject = "Your Beavask Verification Code",
                Body = $"Hello,\n\nHere is your verification code: {verificationCode}\n\nThis code will expire in 3 minutes.\n\nThank you.",
                IsBodyHtml = false
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
        public async Task SendUserCredentialsAsync(string toEmail, string loginName, string password)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Mail:Username"]),
                Subject = "Your Beavask Login Details",
                Body = $"Hello,\n\nHere are your login details:\n\nUsername: {loginName}\nPassword: {password}\n\nThank you.",
                IsBodyHtml = false
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
