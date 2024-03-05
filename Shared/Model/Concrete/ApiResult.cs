using Shared.Model.Abstract;

namespace Shared.Model.Concrete
{
    public class ApiResult : IApiResult
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public int ErrorCode { get; set; }
        public object? Result { get; set; } = null;
    }
}
