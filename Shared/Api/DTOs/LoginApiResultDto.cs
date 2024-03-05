namespace Shared.Api.DTOs
{
    public class LoginApiResultDto
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public int ErrorCode { get; set; }
        public string Result { get; set; } = string.Empty;
    }
}
