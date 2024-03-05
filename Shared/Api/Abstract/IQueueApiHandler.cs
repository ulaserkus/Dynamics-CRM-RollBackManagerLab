using Shared.Api.DTOs;
using Shared.Model.Abstract;

namespace Shared.Api.Abstract
{
    public interface IQueueApiHandler
    {
        Task<IApiResult> AddEntityHistoryAsync(EntityHistoryApiDto historyApiDto);
        Task<IApiResult> EntityHistoryAsync(string id, string userId);
        Task<IApiResult> GetEntityHistoryByConfigurationAsync(GetEntityHistoryByConfigurationApiDto historyApiDto);
        Task<IApiResult> GetEntityHistoryWithoutAtrributesByConfigurationAsync(GetEntityHistoryByConfigurationApiDto historyApiDto);
        Task<IApiResult> DeleteEntityHistoryByConfigurationAsync(Guid configId, string userId);
        Task<IApiResult> DeleteEntityHistoryAsync(string id, string userId);
    }
}
