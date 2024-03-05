using MongoDB.Driver;
using Shared.Model.Abstract;
using System.Linq.Expressions;

namespace Shared.Repository.Abstract
{
    public interface IMongoRepository<TDocument> where TDocument : IMongoDbDocument
    {
        IQueryable<TDocument> AsQueryable();
        IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> expression);
        Task<IEnumerable<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> expression);
        TDocument FindOne(Expression<Func<TDocument, bool>> expression);
        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> expression);
        TDocument FindById(string id);
        Task<TDocument> FindByIdAsync(string id);
        Task<TDocument> FindSingleAsync(FilterDefinition<TDocument> filterDefinition);
        void InsertOne(TDocument document);
        Task InsertOneAsync(TDocument document);
        void InsertMany(ICollection<TDocument> documents);
        Task InsertManyAsync(ICollection<TDocument> documents);
        void ReplaceOne(TDocument document);
        Task ReplaceOneAsync(TDocument document);
        void DeleteOne(Expression<Func<TDocument, bool>> expression);
        Task DeleteOneAsync(Expression<Func<TDocument, bool>> expression);
        void DeleteById(string id);
        Task DeleteByIdAsync(string id);
        void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);
        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task<long> GetDocumentCountByFilter(FilterDefinition<TDocument> filterDefinition);
    }
}
