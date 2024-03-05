using Microsoft.AspNetCore.Http;
using Shared.Model.Concrete;

namespace Queue.Framework.Core.Exception
{
    public class UnauthorizedApiUserException : CustomException
    {
        public UnauthorizedApiUserException(int statusCode = StatusCodes.Status401Unauthorized, string errorMessage = "Access Restricted", int errorCode = StatusCodes.Status401Unauthorized) : base(statusCode, errorMessage, errorCode)
        {
        }
    }
}
