using System.Security.Cryptography;
using System.Text;

namespace AspNetSecurity.Core.Services
{
    public static class PasswordHelper
    {
        private static StringBuilder charPool;

        static PasswordHelper() 
        {
            charPool = new StringBuilder();
            for (char i = 'a'; i <= 'z'; i++)
            {
                charPool.Append(i);
                charPool.Append(Char.ToUpper(i));
            }

            for (char i = '0'; i <= '9'; i++)
            {
                charPool.Append(i);
                charPool.Append(Char.ToUpper(i));
            }

            charPool.Append("_?+*=");
        }

        public static string GeneratePasswordBinary(int byteLength)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] num = new byte[byteLength];
            rng.GetBytes(num);
            return BitConverter.ToString(num).Replace("-", string.Empty);
        }

        public static string GeneratePassword(int passwordLength)
        {
            StringBuilder password = new StringBuilder();
            for (int i = 0; i < passwordLength; i++)
            {
                password.Append(charPool[RandomNumberGenerator.GetInt32(charPool.Length)]);
            }
            return password.ToString();
        }
    }
}
