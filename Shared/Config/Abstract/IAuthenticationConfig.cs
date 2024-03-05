namespace Shared.Config.Abstract
{
    public interface IAuthenticationConfig
    {
        string ApiName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}
