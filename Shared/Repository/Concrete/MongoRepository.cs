using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Config.Abstract;
using Shared.Model.Abstract;
using Shared.Repository.Abstract;
using System.Linq.Expressions;

namespace Shared.Repository.Concrete
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument>
        where TDocument : IMongoDbDocument
    {
        protected readonly IMongoCollection<TDocument> _collection;
        public MongoRepository(IMongoDbSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TDocument>(typeof(TDocument).Name);
        }
        public IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public void DeleteById(string id)
        {
            var objectId = new ObjectId(id);
            var filters = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            _collection.FindOneAndDelete(filters);

        }

        public async Task DeleteByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filters = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            await _collection.FindOneAndDeleteAsync(filters);
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public async Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            await _collection.DeleteManyAsync(filterExpression);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> expression)
        {
            _collection.FindOneAndDelete(expression);
        }

        public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> expression)
        {
            await _collection.FindOneAndDeleteAsync(expression);
        }

        public IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> expression)
        {
            return _collection.Find(expression).ToEnumerable();
        }

        public async Task<IEnumerable<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> expression)
        {
            return (await _collection.FindAsync(expression)).ToEnumerable();
        }

        public TDocument FindById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            return _collection.Find(filter).SingleOrDefault();
        }

        public async Task<TDocument> FindByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            return await (await _collection.FindAsync(filter)).SingleOrDefaultAsync();
        }

        public TDocument FindOne(Expression<Func<TDocument, bool>> expression)
        {
            return _collection.Find(expression).FirstOrDefault();
        }

        public async Task<TDocument> FindSingleAsync(FilterDefinition<TDocument> filterDefinition)
        {
            return (await _collection.FindAsync(filterDefinition)).SingleOrDefault();
        }

        public async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> expression)
        {
            return await (await _collection.FindAsync(expression)).FirstOrDefaultAsync();
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            _collection.InsertMany(documents);
        }

        public async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public void InsertOne(TDocument document)
        {
            _collection.InsertOne(document);
        }

        public async Task InsertOneAsync(TDocument document)
        {
            await _collection.InsertOneAsync(document);
        }

        public void ReplaceOne(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public async Task ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public async Task<long> GetDocumentCountByFilter(FilterDefinition<TDocument> filterDefinition)
        {
            return await _collection.CountDocumentsAsync(filterDefinition);
        }
    }
}
