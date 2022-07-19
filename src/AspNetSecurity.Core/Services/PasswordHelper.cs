using System.Security.Cryptography;

namespace AspNetSecurity.Core.Services
{
    public static class PasswordHelper
    {
        public static string GeneratePassword()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] num = new byte[32];
            rng.GetBytes(num);
            return BitConverter.ToString(num).Replace("-", string.Empty);
        }
    }
}
