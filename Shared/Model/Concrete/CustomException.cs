using Shared.Model.Abstract;

namespace Shared.Model.Concrete
{
    public class CustomException : System.Exception, ICustomException
    {
        public CustomException(int statusCode, string errorMessage, int errorCode) : base(errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        public int StatusCode { get; }
        public int ErrorCode { get; }
        public string ErrorMessage { get; }
    }
}
