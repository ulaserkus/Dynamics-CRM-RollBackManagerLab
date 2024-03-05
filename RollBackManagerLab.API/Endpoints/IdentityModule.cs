using Carter;
using Shared.Api.Abstract;
using Shared.CustomController;
using System.Security.Claims;

namespace RollBackManagerLab.API.Endpoints
{
    public class IdentityModule : CustomBaseController, ICarterModule
    {
        private const string endpointTag = "identity";
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            #region This Endpoints Not Necessary At The Moment

            //app.MapPost($"/{endpointTag}/add", async ([FromBody] AddUserRequestApiDto addUserRequestDto, IIdentityApiHandler apiHandler) =>
            //{
            //    var result = await apiHandler.AddUserAsync(addUserRequestDto);
            //    return SetMinimalApiResult(result);

            //}).RequireAuthorization();

            //app.MapPost($"/{endpointTag}/login", async ([FromBody] LoginApiDto loginDto, IIdentityApiHandler apiHandler) =>
            //{
            //    var result = await apiHandler.LoginAsync(loginDto);
            //    return SetMinimalApiResult(result);
            //});

            #endregion


            app.MapPost($"/{endpointTag}/loginwithkey", async (IHttpContextAccessor httpContextAccessors, IIdentityApiHandler apiHandler) =>
            {
                var apiKey = httpContextAccessors.HttpContext?.Request.Headers["ApiKey"].ToString();

                if (string.IsNullOrEmpty(apiKey)) return Results.BadRequest();

                var result = await apiHandler.LoginWithKeyAsync(apiKey);
                return SetMinimalApiResult(result);

            });

            app.MapPost($"/{endpointTag}/userkeys", async (IHttpContextAccessor httpContextAccessors, IIdentityApiHandler apiHandler) =>
            {
                var user = httpContextAccessors.HttpContext?.User;
                string userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

                if (string.IsNullOrEmpty(userId)) return Results.BadRequest();

                var result = await apiHandler.GetUserKeysByIdAsync(userId);
                return SetMinimalApiResult(result);

            }).RequireAuthorization();
        }
    }
}
