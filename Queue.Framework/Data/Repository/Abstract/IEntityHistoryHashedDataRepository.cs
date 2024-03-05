using MongoDB.Driver;
using Queue.Framework.Data.Model;
using Shared.Repository.Abstract;

namespace Queue.Framework.Data.Repository.Abstract
{
    public interface IEntityHistoryHashedDataRepository : IMongoRepository<EntityHistoryHashedData>
    {
        Task<ICollection<EntityHistoryHashedData>> QueryWithPaginationAsync(FilterDefinition<EntityHistoryHashedData> filter, SortDefinition<EntityHistoryHashedData> sort, int count, int page);
    }
}
