using System.Security.Cryptography;

namespace Shared.Utils
{
    public static class HashUtil
    {
        private const int saltSize = 32;
        private const int hashSize = 32;
        private const int iterationCount = 10000;
        public static string GenerateHash(string data, out string salt)
        {
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(data, saltSize))
            {
                rfc2898DeriveBytes.IterationCount = iterationCount;
                byte[] hashData = rfc2898DeriveBytes.GetBytes(hashSize);
                byte[] saltData = rfc2898DeriveBytes.Salt;
                salt = Convert.ToBase64String(saltData);
                return Convert.ToBase64String(hashData);
            }
        }

        public static bool VerifyHash(string data, string hashedData, string salt)
        {
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(data, saltSize))
            {
                rfc2898DeriveBytes.IterationCount = iterationCount;
                rfc2898DeriveBytes.Salt = Convert.FromBase64String(salt);
                byte[] hashData = rfc2898DeriveBytes.GetBytes(hashSize);
                return Convert.ToBase64String(hashData) == hashedData;
            }
        }
    }
}
