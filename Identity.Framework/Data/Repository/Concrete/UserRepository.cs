using Identity.Framework.Data.Model;
using Identity.Framework.Data.Repository.Abstract;
using MongoDB.Driver;
using Shared.Config.Abstract;
using Shared.Repository.Concrete;

namespace Identity.Framework.Data.Repository.Concrete
{
    internal class UserRepository : MongoRepository<User>, IUserRepository
    {
        public UserRepository(IMongoDbSettings settings) : base(settings)
        {
        }

        public async Task<User> GetUserWithApiKeyAsync(string apiKey)
        {

            var filter = Builders<User>.Filter.ElemMatch(
                x => x.ApiKeys,
                Builders<ApiKey>.Filter.Eq(key => key.Key, apiKey)
            );

            var user = await FindSingleAsync(filter);
            return user;
        }
    }
}
