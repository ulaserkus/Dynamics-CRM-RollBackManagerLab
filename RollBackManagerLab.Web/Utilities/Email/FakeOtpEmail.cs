using Shared.Config.Abstract;

namespace RollBackManagerLab.Web.Utilities.Email
{
    public class FakeOtpEmail : OtpEmail
    {
        public string Code { get; } = "123456";

        public FakeOtpEmail(ISmtpConfig smtpConfig, string htmlName = "otp.html", string subject = "OTP Mail") : base(smtpConfig, htmlName, subject)
        {
        }
        public override void Send(string toEmail, CancellationToken cancellationToken)
        {

        }

        public override async Task SendAsync(string toEmail, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
