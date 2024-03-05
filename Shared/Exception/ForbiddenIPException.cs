using Microsoft.AspNetCore.Http;
using Shared.Model.Concrete;

namespace Shared.Exception
{
    public class ForbiddenIPException : CustomException
    {
        public ForbiddenIPException(int statusCode = StatusCodes.Status403Forbidden, string errorMessage = "Forbidden - IP not allowed.", int errorCode = StatusCodes.Status403Forbidden) : base(statusCode, errorMessage, errorCode)
        {
        }
    }
}
