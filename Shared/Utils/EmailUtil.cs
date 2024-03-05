using Shared.Config.Abstract;
using System.Net;
using System.Net.Mail;


namespace Shared.Utils
{
    public static class EmailUtil
    {
        public static bool SendMailWithSmtp(ISmtpConfig smtpConfig, string subject, string body, string toEmail)
        {
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(smtpConfig.Email),
                To = { toEmail },
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            SmtpClient smtpClient = new SmtpClient()
            {
                Host = smtpConfig.Host,
                Port = smtpConfig.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpConfig.Email, smtpConfig.Password)
            };

            try
            {
                // Send the email
                smtpClient.Send(mail);
            }
            catch (System.Exception)
            {
                return false;
            }
            finally
            {
                mail.Dispose();
                smtpClient.Dispose();
            }

            return true;
        }

        public static async Task<bool> SendMailWithSmtpAsync(ISmtpConfig smtpConfig, string subject, string body, string toEmail)
        {
            using MailMessage mail = new MailMessage()
            {
                From = new MailAddress(smtpConfig.Email),
                To = { toEmail },
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            using SmtpClient smtpClient = new SmtpClient()
            {
                Host = smtpConfig.Host,
                Port = smtpConfig.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpConfig.Email, smtpConfig.Password)
            };

            try
            {
                // Send the email
                await smtpClient.SendMailAsync(mail);
            }
            catch (System.Exception)
            {
                return false;
            }

            return true;
        }
    }
}
