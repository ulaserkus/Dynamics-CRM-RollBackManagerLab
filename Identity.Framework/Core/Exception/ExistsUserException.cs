using Shared.Model.Concrete;

namespace Identity.Framework.Core.Exception
{
    internal class ExistsUserException : CustomException
    {
        public ExistsUserException(int statusCode, string errorMessage, int errorCode) : base(statusCode, errorMessage, errorCode)
        {
        }
    }
}
