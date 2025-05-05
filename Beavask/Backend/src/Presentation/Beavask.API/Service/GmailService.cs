using Beavask.Application.Interface.Service;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

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
            try
            {
                if (string.IsNullOrWhiteSpace(toEmail))
                    throw new ArgumentNullException(nameof(toEmail), "Recipient email address cannot be null or empty.");

                var fromEmail = _configuration["Mail:Username"];
                var password = _configuration["Mail:Password"];

                if (string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(password))
                    throw new InvalidOperationException("Email configuration values are missing (Mail:Username or Mail:Password).");

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = true
                };

                var logoPath = _configuration["Logo:Path"];
                var logoCid = "logo_cid"; // Inline olarak gösterilecek CID

                string bodyHtml = $@"
                <html>
                  <body style='font-family: Arial, sans-serif; background-color: #f3f4f6; padding: 40px;'>
                    <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 12px; box-shadow: 0 4px 14px rgba(0,0,0,0.08); overflow: hidden;'>
                      <div style='background-color: #facc15; padding: 30px; text-align: center;'>
                        <img src='cid:{logoCid}' alt='Beavask Logo' style='height: 90px; margin-bottom: 10px;' />
                      </div>
                      <div style='padding: 35px 30px;'>
                        <h2 style='color: #111827; margin-bottom: 10px;'>Your Verification Code</h2>
                        <p style='font-size: 16px; color: #4b5563;'>Use the following code to verify your Beavask account. This code will expire in 3 minutes:</p>
                        <div style='margin-top: 25px; background-color: #f3f4f6; padding: 20px; border-radius: 8px; text-align: center; font-size: 24px; font-weight: bold; color: #1f2937; letter-spacing: 3px;'>
                          {verificationCode}
                        </div>
                        <p style='margin-top: 30px; font-size: 14px; color: #6b7280;'>If the code expires, you can request a new one.</p>
                        <p style='margin-top: 40px; font-size: 14px; color: #6b7280;'>Thank you,<br/>The Beavask Team</p>
                      </div>
                    </div>
                  </body>
                </html>";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Your Beavask Verification Code",
                    Body = bodyHtml,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                // Inline görseli ekliyoruz
                var logoAttachment = new Attachment(logoPath)
                {
                    ContentId = logoCid,
                    ContentDisposition = { Inline = true }
                };
                mailMessage.Attachments.Add(logoAttachment);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to send verification email: {ex.Message}");
                throw;
            }
        }
        public async Task SendUserCredentialsAsync(string toEmail, string loginName, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(toEmail))
                    throw new ArgumentNullException(nameof(toEmail), "Recipient email address cannot be null or empty.");

                var fromEmail = _configuration["Mail:Username"];
                var passwordConfig = _configuration["Mail:Password"];

                if (string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(passwordConfig))
                    throw new InvalidOperationException("Email configuration values are missing (Mail:Username or Mail:Password).");

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, passwordConfig),
                    EnableSsl = true
                };

                var logoPath = _configuration["Logo:Path"];
                var logoCid = "logo_cid"; // Inline görsel CID'si

                string bodyHtml = $@"
                <html>
                  <body style='font-family: Arial, sans-serif; background-color: #f3f4f6; padding: 40px;'>
                    <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 12px; box-shadow: 0 4px 14px rgba(0,0,0,0.08); overflow: hidden;'>
                      <div style='background-color: #facc15; padding: 30px; text-align: center;'>
                        <img src='cid:{logoCid}' alt='Beavask Logo' style='height: 90px; margin-bottom: 10px;' />
                      </div>
                      <div style='padding: 35px 30px;'>
                        <h2 style='color: #111827; margin-bottom: 10px;'>Your Login Credentials</h2>
                        <p style='font-size: 16px; color: #4b5563;'>You can log in to the Beavask system using the following credentials:</p>
                        <div style='margin-top: 25px; background-color: #f3f4f6; padding: 20px; border-radius: 8px;'>
                          <p style='margin: 5px 0; font-size: 16px;'><strong>Username:</strong> {loginName}</p>
                          <p style='margin: 5px 0; font-size: 16px;'><strong>Password:</strong> {password}</p>
                        </div>
                        <p style='margin-top: 30px; font-size: 14px; color: #6b7280;'>Click <a href='https://beavask.com/login' style='color: #1d4ed8;'>here</a> to log in.</p>
                        <p style='margin-top: 30px; font-size: 14px; color: #6b7280;'>Thank you,<br/>The Beavask Team</p>
                      </div>
                    </div>
                  </body>
                </html>";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Your Beavask Login Details",
                    Body = bodyHtml,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                // Inline görseli ekliyoruz
                var logoAttachment = new Attachment(logoPath)
                {
                    ContentId = logoCid,
                    ContentDisposition = { Inline = true }
                };
                mailMessage.Attachments.Add(logoAttachment);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to send credentials email: {ex.Message}");
                throw;
            }
        }
        public async Task SendIndividualVerificationCodeAsync(string toEmail, string verificationCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(toEmail))
                    throw new ArgumentNullException(nameof(toEmail), "Recipient email address cannot be null or empty.");

                var fromEmail = _configuration["Mail:Username"];
                var password = _configuration["Mail:Password"];

                if (string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(password))
                    throw new InvalidOperationException("Email configuration values are missing (Mail:Username or Mail:Password).");

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = true
                };

                var logoPath = _configuration["Logo:Path"];
                var logoCid = "logo_cid"; // Inline görsel CID'si

                string bodyHtml = $@"
                <html>
                  <body style='font-family: Arial, sans-serif; background-color: #f3f4f6; padding: 40px;'>
                    <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 12px; box-shadow: 0 4px 14px rgba(0,0,0,0.08); overflow: hidden;'>
                      <div style='background-color: #facc15; padding: 30px; text-align: center;'>
                        <img src='cid:{logoCid}' alt='Beavask Logo' style='height: 90px; margin-bottom: 10px;' />
                      </div>
                      <div style='padding: 35px 30px;'>
                        <h2 style='color: #111827; margin-bottom: 10px;'>Welcome to Beavask, {toEmail.Split('@')[0]}!</h2>
                        <p style='font-size: 16px; color: #4b5563;'>Thank you for registering with Beavask. Use the following verification code to complete your registration:</p>
                        <div style='margin-top: 25px; background-color: #f3f4f6; padding: 20px; border-radius: 8px; text-align: center; font-size: 24px; font-weight: bold; color: #1f2937; letter-spacing: 3px;'>
                          {verificationCode}
                        </div>
                        <p style='margin-top: 30px; font-size: 14px; color: #6b7280;'>This code will expire in 3 minutes. If it expires, feel free to request a new one.</p>
                        <p style='margin-top: 40px; font-size: 14px; color: #6b7280;'>Best regards,<br/>The Beavask Team</p>
                      </div>
                    </div>
                  </body>
                </html>";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Your Beavask Verification Code",
                    Body = bodyHtml,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                // Inline görseli ekliyoruz
                var logoAttachment = new Attachment(logoPath)
                {
                    ContentId = logoCid,
                    ContentDisposition = { Inline = true }
                };
                mailMessage.Attachments.Add(logoAttachment);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to send verification email: {ex.Message}");
                throw;
            }
        }
        public async Task SendRegistrationSuccessEmailAsync(string toEmail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(toEmail))
                    throw new ArgumentNullException(nameof(toEmail), "Recipient email address cannot be null or empty.");

                var fromEmail = _configuration["Mail:Username"];
                var password = _configuration["Mail:Password"];

                if (string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(password))
                    throw new InvalidOperationException("Email configuration values are missing (Mail:Username or Mail:Password).");

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = true
                };

                var logoPath = _configuration["Logo:Path"];
                var logoCid = "logo_cid"; 

                string bodyHtml = $@"
                <html>
                  <body style='font-family: Arial, sans-serif; background-color: #f3f4f6; padding: 40px;'>
                    <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 12px; box-shadow: 0 4px 14px rgba(0,0,0,0.08); overflow: hidden;'>
                      <div style='background-color: #10b981; padding: 30px; text-align: center;'>
                        <img src='cid:{logoCid}' alt='Beavask Logo' style='height: 90px; margin-bottom: 10px;' />
                      </div>
                      <div style='padding: 35px 30px;'>
                        <h2 style='color: #111827; margin-bottom: 10px;'>Registration Successful!</h2>
                        <p style='font-size: 16px; color: #4b5563;'>Thank you for registering with Beavask. Your account has been successfully created and is now ready to use. We’re excited to have you on board!</p>
                        <p style='margin-top: 30px; font-size: 14px; color: #6b7280;'>If you have any questions or need assistance, feel free to reach out to us. We’re always happy to help!</p>
                        <p style='margin-top: 40px; font-size: 14px; color: #6b7280;'>Best regards,<br/>The Beavask Team</p>
                      </div>
                    </div>
                  </body>
                </html>";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Your Beavask Registration Was Successful",
                    Body = bodyHtml,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                var logoAttachment = new Attachment(logoPath)
                {
                    ContentId = logoCid,
                    ContentDisposition = { Inline = true }
                };
                mailMessage.Attachments.Add(logoAttachment);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to send registration success email: {ex.Message}");
                throw;
            }
        }
    }
}
