using Microsoft.AspNetCore.Http;
using Shared.Model.Concrete;

namespace Shared.Exception
{
    public class UnAuthorizedException : CustomException
    {
        public UnAuthorizedException(int statusCode = StatusCodes.Status401Unauthorized, string errorMessage = "Invalid Authentication", int errorCode = StatusCodes.Status401Unauthorized) : base(statusCode, errorMessage, errorCode)
        {
        }
    }
}
