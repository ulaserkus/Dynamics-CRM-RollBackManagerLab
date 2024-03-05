using MongoDB.Driver;
using Queue.Framework.Data.Model;
using Queue.Framework.Data.Repository.Abstract;
using Shared.Config.Abstract;
using Shared.Repository.Concrete;

namespace Queue.Framework.Data.Repository.Concrete
{
    public class EntityHistoryHashedDataRepository : MongoRepository<EntityHistoryHashedData>, IEntityHistoryHashedDataRepository
    {
        public EntityHistoryHashedDataRepository(IMongoDbSettings settings) : base(settings)
        {
        }
        public async Task<ICollection<EntityHistoryHashedData>> QueryWithPaginationAsync(FilterDefinition<EntityHistoryHashedData> filter, SortDefinition<EntityHistoryHashedData> sort, int count, int page)
        {
            int skip = (page - 1) * count;

            var entities = await _collection.Find(filter).Sort(sort).Skip(skip).Limit(count).ToListAsync();

            return entities;
        }
    }
}
