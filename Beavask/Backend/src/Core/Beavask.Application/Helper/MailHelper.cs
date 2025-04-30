namespace Beavask.Application.Helper;
public class MailHelper
{
    public static string GenerateVerificationCode()
    {
        var random = new Random();
        var code = random.Next(100000, 999999).ToString();
        return code;
    }
}
