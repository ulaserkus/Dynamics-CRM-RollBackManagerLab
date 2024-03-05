namespace Shared.Model.Abstract
{
    public interface ICustomException
    {
        int StatusCode { get; }
        int ErrorCode { get; }
        string ErrorMessage { get; }
    }
}
