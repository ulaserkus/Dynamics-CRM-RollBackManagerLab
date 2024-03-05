namespace Identity.Framework.Core.DTOs
{
    public class LoginUserRequestDto
    {
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
