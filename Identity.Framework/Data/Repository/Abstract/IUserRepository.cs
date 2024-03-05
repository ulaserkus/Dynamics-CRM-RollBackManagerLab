using Identity.Framework.Data.Model;
using Shared.Repository.Abstract;

namespace Identity.Framework.Data.Repository.Abstract
{
    public interface IUserRepository : IMongoRepository<User>
    {
        Task<User> GetUserWithApiKeyAsync(string apiKey);
    }
}
