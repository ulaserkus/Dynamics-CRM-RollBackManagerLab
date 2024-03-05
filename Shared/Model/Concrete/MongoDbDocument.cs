using MongoDB.Bson;
using Shared.Model.Abstract;

namespace Shared.Model.Concrete
{
    public class MongoDbDocument : IMongoDbDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;
    }
}
