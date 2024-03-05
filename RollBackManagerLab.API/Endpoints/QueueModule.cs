using Carter;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Abstract;
using Shared.Api.DTOs;
using Shared.CustomController;
using System.Security.Claims;

namespace RollBackManagerLab.API.Endpoints
{
    public class QueueModule : CustomBaseController, ICarterModule
    {
        private const string endpointTag = "queue";
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet($"/{endpointTag}/history/" + "{id}", async ([FromRoute] string id, IQueueApiHandler apiHandler, IHttpContextAccessor httpContextAccessors) =>
            {
                var user = httpContextAccessors.HttpContext?.User;
                string userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

                if (string.IsNullOrEmpty(userId)) return Results.BadRequest();

                var result = await apiHandler.EntityHistoryAsync(id, userId);
                return SetMinimalApiResult(result);

            }).RequireAuthorization();

            app.MapPost($"/{endpointTag}/add", async ([FromBody] EntityHistoryApiDto entityHistoryApiDto, IQueueApiHandler apiHandler, IHttpContextAccessor httpContextAccessors) =>
            {
                var user = httpContextAccessors.HttpContext?.User;
                string userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

                if (string.IsNullOrEmpty(userId)) return Results.BadRequest();
                entityHistoryApiDto.UserId = userId;

                var result = await apiHandler.AddEntityHistoryAsync(entityHistoryApiDto);
                return SetMinimalApiResult(result);

            }).RequireAuthorization();

            app.MapPost($"/{endpointTag}/querybyconfiguration", async ([FromBody] GetEntityHistoryByConfigurationApiDto entityHistoryApiDto, IQueueApiHandler apiHandler, IHttpContextAccessor httpContextAccessors) =>
            {
                var user = httpContextAccessors.HttpContext?.User;
                string userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

                if (string.IsNullOrEmpty(userId)) return Results.BadRequest();
                entityHistoryApiDto.UserId = userId;

                var result = await apiHandler.GetEntityHistoryByConfigurationAsync(entityHistoryApiDto);
                return SetMinimalApiResult(result);

            }).RequireAuthorization();

            app.MapPost($"/{endpointTag}/querywithoutattributes", async ([FromBody] GetEntityHistoryByConfigurationApiDto entityHistoryApiDto, IQueueApiHandler apiHandler, IHttpContextAccessor httpContextAccessors) =>
            {
                var user = httpContextAccessors.HttpContext?.User;
                string userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

                if (string.IsNullOrEmpty(userId)) return Results.BadRequest();
                entityHistoryApiDto.UserId = userId;

                var result = await apiHandler.GetEntityHistoryWithoutAtrributesByConfigurationAsync(entityHistoryApiDto);
                return SetMinimalApiResult(result);

            }).RequireAuthorization();

            app.MapDelete($"/{endpointTag}/deletebyconfiguration/" + "{configurationId}", async ([FromRoute] Guid configurationId, IQueueApiHandler apiHandler, IHttpContextAccessor httpContextAccessors) =>
            {
                var user = httpContextAccessors.HttpContext?.User;
                string userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

                if (string.IsNullOrEmpty(userId)) return Results.BadRequest();

                var result = await apiHandler.DeleteEntityHistoryByConfigurationAsync(configurationId, userId);
                return SetMinimalApiResult(result);

            }).RequireAuthorization();

            app.MapDelete($"/{endpointTag}/delete/" + "{id}", async ([FromRoute] string id, IQueueApiHandler apiHandler, IHttpContextAccessor httpContextAccessors) =>
            {
                var user = httpContextAccessors.HttpContext?.User;
                string userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

                if (string.IsNullOrEmpty(userId)) return Results.BadRequest();

                var result = await apiHandler.DeleteEntityHistoryAsync(id, userId);
                return SetMinimalApiResult(result);

            }).RequireAuthorization();
        }
    }
}
