using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Queue.Framework.Core.Mapper.Abstract;
using Queue.Framework.Core.Services.Abstract;
using Queue.Framework.Data.Model;
using Queue.Framework.Data.Repository.Abstract;
using Shared.Api.DTOs;
using Shared.Model.Abstract;
using Shared.Model.Concrete;
using Shared.Utils;
using System.Dynamic;
using System.Text.Json;

namespace Queue.Framework.Core.Services.Concrete
{
    public class EntityHistoryHashedDataService : IEntityHistoryHashedDataService
    {
        private readonly IEntityHistoryHashedDataRepository _repository;
        private readonly IEntityHistoryApiDtoMapper _entityHistoryMapper;

        public EntityHistoryHashedDataService(IEntityHistoryHashedDataRepository repository, IEntityHistoryApiDtoMapper entityHistoryMapper)
        {
            _repository = repository;
            _entityHistoryMapper = entityHistoryMapper;
        }

        public async Task<IApiResult> DeleteEntityHistoryAsync(string id, string userId)
        {
            await _repository.DeleteOneAsync(x => x.Id == ObjectId.Parse(id) && x.UserId == userId);

            return new ApiResult
            {
                StatusCode = StatusCodes.Status204NoContent,
                Result = true
            };
        }

        public async Task<IApiResult> GetEntityHistoryAsync(string id, string userId)
        {
            var filter = Builders<EntityHistoryHashedData>.Filter.And(
      Builders<EntityHistoryHashedData>.Filter.Eq(x => x.Id, ObjectId.Parse(id)),
      Builders<EntityHistoryHashedData>.Filter.Eq(x => x.UserId, userId));

            var historyData = await _repository.FindSingleAsync(filter);

            var result = _entityHistoryMapper.ResolveHashedEntityHistory(historyData);

            return new ApiResult
            {
                StatusCode = StatusCodes.Status200OK,
                Result = result
            };
        }

        public async Task<IApiResult> DeleteEntityHistoryByConfigurationAsync(Guid configId, string userId)
        {
            await _repository.DeleteManyAsync(x => x.ConfigurationId == configId && x.UserId == userId);

            return new ApiResult
            {
                StatusCode = StatusCodes.Status204NoContent,
                Result = true
            };
        }


        public async Task<IApiResult> GetEntityHistoryByConfigurationAsync(GetEntityHistoryByConfigurationApiDto entityHistoryByConfigurationApiDto)
        {
            var filter = Builders<EntityHistoryHashedData>.Filter.And(
     Builders<EntityHistoryHashedData>.Filter.Eq(x => x.ConfigurationId, entityHistoryByConfigurationApiDto.ConfigurationId),
     Builders<EntityHistoryHashedData>.Filter.Eq(x => x.UserId, entityHistoryByConfigurationApiDto.UserId));

            var sort = Builders<EntityHistoryHashedData>.Sort.Descending(x => x.OperationDate);

            var countTask = _repository.GetDocumentCountByFilter(filter);

            var result = await _repository.QueryWithPaginationAsync(filter, sort, entityHistoryByConfigurationApiDto.Count, entityHistoryByConfigurationApiDto.Page);

            var mappedResult = result.Select(x => _entityHistoryMapper.ResolveHashedEntityHistory(x)).ToList();

            dynamic dynamicResult = new ExpandoObject();
            dynamicResult.TotalRecordCount = await countTask;
            dynamicResult.Data = mappedResult;

            return new ApiResult
            {
                StatusCode = StatusCodes.Status200OK,
                Result = dynamicResult
            };
        }

        public async Task<IApiResult> GetEntityHistoryWithoutAttributesByConfigurationAsync(GetEntityHistoryByConfigurationApiDto entityHistoryByConfigurationApiDto)
        {
            var filter = Builders<EntityHistoryHashedData>.Filter.And(
  Builders<EntityHistoryHashedData>.Filter.Eq(x => x.ConfigurationId, entityHistoryByConfigurationApiDto.ConfigurationId),
  Builders<EntityHistoryHashedData>.Filter.Eq(x => x.UserId, entityHistoryByConfigurationApiDto.UserId));

            var sort = Builders<EntityHistoryHashedData>.Sort.Descending(x => x.OperationDate);

            var countTask = _repository.GetDocumentCountByFilter(filter);

            var result = await _repository.QueryWithPaginationAsync(filter, sort, entityHistoryByConfigurationApiDto.Count, entityHistoryByConfigurationApiDto.Page);

            var mappedResult = result.Select(x => _entityHistoryMapper.MapWithoutAttributesFromHashedData(x)).ToList();

            dynamic dynamicResult = new ExpandoObject();
            dynamicResult.TotalRecordCount = await countTask;
            dynamicResult.Data = mappedResult;

            return new ApiResult
            {
                StatusCode = StatusCodes.Status200OK,
                Result = dynamicResult
            };
        }

        public async Task<IApiResult> InsertEntitHistoryAsync(EntityHistoryApiDto entityHistory)
        {
            EntityHistoryHashedData hashedData = new();

            hashedData.ConfigurationId = entityHistory.ConfigurationId;
            hashedData.RecordUniqueIdentifier = entityHistory.RecordUniqueIdentifier;
            hashedData.ExecutionOwnerId = entityHistory.ExecutionOwnerId;
            hashedData.OperationDate = entityHistory.OperationDate;
            hashedData.LogicalName = entityHistory.LogicalName;
            hashedData.OperationType = entityHistory.OperationType;
            hashedData.UserId = entityHistory.UserId;

            var jsonString = JsonSerializer.Serialize(entityHistory.Attributes);
            hashedData.CrmHashedData = EncryptUtil.Encrypt(jsonString);

            await _repository.InsertOneAsync(hashedData);
            return new ApiResult { StatusCode = StatusCodes.Status201Created };
        }
    }
}
