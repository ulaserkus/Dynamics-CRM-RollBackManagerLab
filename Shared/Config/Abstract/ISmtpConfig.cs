namespace Shared.Config.Abstract
{
    public interface ISmtpConfig
    {
        string Host { get; set; }
        int Port { get; set; }
        string Email { get; set; }
        string Password { get; set; }
    }
}
