using Shared.Config.Abstract;
using Shared.Utils;

namespace RollBackManagerLab.Web.Utilities.Email
{
    public abstract class EmailBase
    {
        private readonly ISmtpConfig _smtpConfig;
        private readonly string _subject;
        private string _body;

        protected EmailBase(ISmtpConfig smtpConfig, string htmlName, string subject)
        {
            _smtpConfig = smtpConfig;
            _subject = subject;
            _body = File.ReadAllText(Path.Combine("wwwroot", "html", htmlName));
        }

        public virtual void Send(string toEmail, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;
            EmailUtil.SendMailWithSmtp(_smtpConfig, _subject, _body, toEmail);
        }

        public virtual async Task SendAsync(string toEmail, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;
            await EmailUtil.SendMailWithSmtpAsync(_smtpConfig, _subject, _body, toEmail);
        }

        public void ReplaceHtmlTagWithValue(string tagName, string value)
        {
            _body = _body.Replace(tagName, value);
        }
    }
}
