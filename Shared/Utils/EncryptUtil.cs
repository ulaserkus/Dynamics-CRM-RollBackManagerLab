using System.Security.Cryptography;
using System.Text;


namespace Shared.Utils
{
    public static class EncryptUtil
    {
        private static string key = "5291e285daadf65951c459ac4fa03f'2";

        public static string Encrypt(string toEncrypt)
        {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.CBC;

                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (ICryptoTransform cTransform = aes.CreateEncryptor())
                {
                    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    byte[] combinedIvCiphertext = iv.Concat(resultArray).ToArray();

                    return Convert.ToBase64String(combinedIvCiphertext);
                }
            }
        }

        public static string Decrypt(string toDecrypt)
        {
            byte[] combinedIvCiphertext = Convert.FromBase64String(toDecrypt);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.CBC;

                byte[] iv = combinedIvCiphertext.Take(16).ToArray();
                aes.IV = iv;

                using (ICryptoTransform cTransform = aes.CreateDecryptor())
                {
                    byte[] resultArray = cTransform.TransformFinalBlock(combinedIvCiphertext, 16, combinedIvCiphertext.Length - 16);
                    return Encoding.UTF8.GetString(resultArray);
                }
            }
        }


    }

}
