using Shared.Config.Abstract;

namespace RollBackManagerLab.Web.Utilities.Email
{
    public class RegisterOtpEmail : OtpEmail
    {
        public RegisterOtpEmail(ISmtpConfig smtpConfig, string htmlName = "registerotp.html", string subject = "Registration OTP Mail") : base(smtpConfig, htmlName, subject)
        {
        }
    }
}
