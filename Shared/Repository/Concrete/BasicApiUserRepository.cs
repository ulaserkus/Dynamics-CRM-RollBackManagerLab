using Shared.Config.Abstract;
using Shared.Model.Concrete;
using Shared.Repository.Abstract;

namespace Shared.Repository.Concrete
{
    public class BasicApiUserRepository : MongoRepository<BasicApiUser>, IBasicApiUserRepository
    {
        public BasicApiUserRepository(IMongoDbSettings settings) : base(settings)
        {
        }
    }
}
