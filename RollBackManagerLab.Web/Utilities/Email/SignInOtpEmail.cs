using Shared.Config.Abstract;

namespace RollBackManagerLab.Web.Utilities.Email
{
    public class SignInOtpEmail : OtpEmail
    {
        public SignInOtpEmail(ISmtpConfig smtpConfig, string htmlName = "signinotp.html", string subject = "Sign In OTP Mail") : base(smtpConfig, htmlName, subject)
        {
        }
    }
}
