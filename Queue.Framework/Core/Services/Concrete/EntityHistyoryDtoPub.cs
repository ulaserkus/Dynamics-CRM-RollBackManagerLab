using MassTransit;
using Microsoft.AspNetCore.Http;
using Queue.Framework.Core.Services.Abstract;
using Shared.Api.DTOs;
using Shared.Model.Abstract;
using Shared.Model.Concrete;

namespace Queue.Framework.Core.Services.Concrete
{
    public class EntityHistyoryDtoPub : QueuePub<EntityHistoryApiDto>, IEntityHistoryDtoPub
    {
        public EntityHistyoryDtoPub(IPublishEndpoint publishEndpoint) : base(publishEndpoint)
        {
        }
        public async Task<IApiResult> AddDataToQueueReturnApiResultAsync(EntityHistoryApiDto data)
        {
            await AddDataToQueueAsync(data);

            return new ApiResult { StatusCode = StatusCodes.Status201Created };
        }
    }
}
