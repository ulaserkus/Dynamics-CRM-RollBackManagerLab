using Microsoft.AspNetCore.Http;
using Shared.Model.Abstract;
using Shared.Model.Concrete;
using System.Text.Json;

namespace Shared.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = GetStatusCode(exception);
            var errCode = GetErrorCode(exception);
            context.Response.StatusCode = statusCode;

            var errorResponse = new ApiResult
            {
                ErrorCode = errCode,
                ErrorMessage = exception.Message,
                StatusCode = statusCode
            };

            var result = JsonSerializer.Serialize(errorResponse);
            return context.Response.WriteAsync(result);
        }

        private static int GetStatusCode(System.Exception exception)
        {
            if (exception is ICustomException customException)
            {
                return customException.StatusCode;
            }
            else
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        private static int GetErrorCode(System.Exception exception)
        {
            if (exception is ICustomException customException)
            {
                return customException.ErrorCode;
            }
            else
            {
                return StatusCodes.Status500InternalServerError;
            }
        }
    }
}
