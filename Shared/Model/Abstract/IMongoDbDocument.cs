using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Model.Abstract
{
    public interface IMongoDbDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }

        DateTime CreatedAt { get; }
    }
}
