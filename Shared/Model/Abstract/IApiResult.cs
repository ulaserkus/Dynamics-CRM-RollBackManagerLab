namespace Shared.Model.Abstract
{
    public interface IApiResult
    {
        int StatusCode { get; set; }
        string ErrorMessage { get; set; }
        int ErrorCode { get; set; }
        object? Result { get; set; }
    }
}
