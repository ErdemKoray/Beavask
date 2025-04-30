using System.Security.Cryptography;
using System.Text;

namespace Beavask.Application.Helper
{
    public class PasswordHelper
    {
        public static void CreatePasswordHash(string password, out string hash, out string salt)
        {
            using var hmac = new HMACSHA256();
            salt = Convert.ToBase64String(hmac.Key);
            hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
        public static bool VerifyPassword(string password, string hash, string salt)
        {
            using var hmac = new HMACSHA256(Convert.FromBase64String(salt));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(computedHash) == hash;
        }
        public static string GenerateRandomPassword()
        {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}