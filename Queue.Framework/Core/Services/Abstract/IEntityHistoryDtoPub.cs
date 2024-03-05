using Shared.Api.DTOs;
using Shared.Model.Abstract;

namespace Queue.Framework.Core.Services.Abstract
{
    public interface IEntityHistoryDtoPub : IQueuePub<EntityHistoryApiDto>
    {
        Task<IApiResult> AddDataToQueueReturnApiResultAsync(EntityHistoryApiDto data);
    }
}
