using Shared.Utils;
using System.Text;

namespace Shared.Contants
{
    public class AppSecureConstants
    {
        /// <summary>
        /// Identity API auth
        /// </summary>
        public const string IdentityApiUserName = "Identity.WebApiSecureUserN@me1";
        public const string IdentityApiPasswordCrypted = "Y9DSd2kWtZWdYLCw5aBuGZwd1Ho+6LHygWKR7Xv6ot03yqo7CgY4q2XrNcz8SgDb";
        public const string IdentityApiClientName = "Identity.WebApi";
        public static string IdentityAPICredential => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{IdentityApiUserName}:{EncryptUtil.Decrypt(IdentityApiPasswordCrypted)}"));

        /// <summary>
        /// QUEUE API auth
        /// </summary>
        public const string QueueApiUserName = "Queue.WebApiSecureUserN@me1";
        public const string QueuePasswordCrypted = "9d0RUJvGA0HI7OfmT+1RO636fk0FH0OoMFJsKoxvFDFqpsjr79BMwKn2NkoB4/hT";
        public const string QueueApiClientName = "Queue.WebApi";
        public static string QueueCredential => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{QueueApiUserName}:{EncryptUtil.Decrypt(QueuePasswordCrypted)}"));

        /// <summary>
        /// JWT auth
        /// </summary>
        public const string JwtIssuer = "Identity.WebApi";
        public const string JwtAudience = "RollBackManager.API";
        public const string JwtSecretCrypted = "lhBi7EwLxHCulGC55/sVnpvk3Ar70bjCa6jHi5lwfy8UUH1nun/TUPthBBOgI3PVs8rWVNI3hnNleUDgo17P9LAbHF50Qc09SzkTfvpLas0X6GsakDWocefAF4Dl8xVJvqJjcnVlOZlrmlj+nKLtxNJs7A8h2BAp5bp/WRsAaf3cFheNjLRJLaNgYR3IQ74WAO05Cj1m2tBF3/Df4PHLdA==";
        public static string JwtSecret => EncryptUtil.Decrypt(JwtSecretCrypted);

        protected AppSecureConstants() { }
    }
}
