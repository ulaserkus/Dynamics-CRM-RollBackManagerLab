namespace Shared.Utils
{
    public class OTPGenerator
    {
        private static readonly Random _random = new Random();
        protected OTPGenerator() { }
        public static string GenerateOTP()
        {
            const int otpLength = 6;
            const string chars = "0123456789";
            char[] otp = new char[otpLength];

            lock (_random)
            {
                for (int i = 0; i < otpLength; i++)
                {
                    otp[i] = chars[_random.Next(chars.Length)];
                }
            }

            return new string(otp);
        }
    }
}
