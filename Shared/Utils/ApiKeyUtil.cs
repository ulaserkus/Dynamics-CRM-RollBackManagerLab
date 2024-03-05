using System.Security.Cryptography;
using System.Text;

namespace Shared.Utils
{
    public static class ApiKeyUtil
    {
        public static string GenerateApiKey(string emailAddress)
        {
            string randomValue = GenerateRandomValue();
            byte[] hashedBytes = HashData(emailAddress + randomValue);
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        private static string GenerateRandomValue()
        {
            return Guid.NewGuid().ToString();
        }

        private static byte[] HashData(string data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            }
        }
    }

}
