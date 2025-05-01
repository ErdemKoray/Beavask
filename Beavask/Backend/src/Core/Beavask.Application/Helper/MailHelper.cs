using System.Security.Cryptography;

namespace Beavask.Application.Helper
{
    public class MailHelper
    {
        public static string GenerateVerificationCode()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);

            int value = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 900000 + 100000;

            return value.ToString();
        }
    }
}
