using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Model.Abstract;

namespace Shared.CustomController
{
    public class CustomBaseController : ControllerBase
    {

        public IActionResult CreateActionResultInstance(IApiResult apiResult)
        {
            return new ObjectResult(apiResult)
            {
                StatusCode = apiResult.StatusCode,
            };
        }

        public IResult SetMinimalApiResult(IApiResult apiResult)
        {
            return Results.Json(apiResult, null, "application/json", apiResult.StatusCode);
        }
    }
}
