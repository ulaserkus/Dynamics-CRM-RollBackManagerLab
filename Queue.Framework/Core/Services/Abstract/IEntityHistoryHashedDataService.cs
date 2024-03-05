using Shared.Api.DTOs;
using Shared.Model.Abstract;

namespace Queue.Framework.Core.Services.Abstract
{
    public interface IEntityHistoryHashedDataService
    {
        Task<IApiResult> InsertEntitHistoryAsync(EntityHistoryApiDto entityHistory);
        Task<IApiResult> GetEntityHistoryByConfigurationAsync(GetEntityHistoryByConfigurationApiDto entityHistoryByConfigurationApiDto);
        Task<IApiResult> GetEntityHistoryWithoutAttributesByConfigurationAsync(GetEntityHistoryByConfigurationApiDto entityHistoryByConfigurationApiDto);
        Task<IApiResult> GetEntityHistoryAsync(string id, string userId);
        Task<IApiResult> DeleteEntityHistoryByConfigurationAsync(Guid configId, string userId);
        Task<IApiResult> DeleteEntityHistoryAsync(string id, string userId);
    }
}
