using Shared.Config.Abstract;
using Shared.Utils;

namespace RollBackManagerLab.Web.Utilities.Email
{
    public class OtpEmail : EmailBase
    {
        public string Code { get; }

        public OtpEmail(ISmtpConfig smtpConfig, string htmlName = "otp.html", string subject = "OTP Mail") : base(smtpConfig, htmlName, subject)
        {
            Code = OTPGenerator.GenerateOTP();
            ReplaceHtmlTagWithValue("[OTP Code]", Code);
        }
    }
}
