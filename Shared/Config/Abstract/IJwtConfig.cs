namespace Shared.Config.Abstract
{
    public interface IJwtConfig
    {
        string Secret { get; set; }
        string Issuer { get; set; }
        string Audience { get; set; }
    }
}
