using System;

namespace Beavask.Application.Helper
{
    public static class InvitationTokenHelper
    {
        public static string GenerateToken()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static DateTime GetExpiryDate(int validityInDays = 2)
        {
            return DateTime.UtcNow.AddDays(validityInDays);
        }

        public static DateTime GetCreatedDate()
        {
            return DateTime.UtcNow;
        }
    }
}
