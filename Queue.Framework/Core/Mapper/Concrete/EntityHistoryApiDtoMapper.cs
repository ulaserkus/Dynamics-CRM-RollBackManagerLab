using Queue.Framework.Core.Mapper.Abstract;
using Queue.Framework.Data.Model;
using Riok.Mapperly.Abstractions;
using Shared.Api.DTOs;
using Shared.Utils;
using System.Text.Json;
namespace Queue.Framework.Core.Mapper.Concrete
{
    [Mapper]
    public partial class EntityHistoryApiDtoMapper : IEntityHistoryApiDtoMapper
    {
        public partial EntityHistoryApiDto MapWithoutAttributesFromHashedData(EntityHistoryHashedData entityHistoryHashedData);

        public EntityHistoryApiDto ResolveHashedEntityHistory(EntityHistoryHashedData entityHistoryHashedData)
        {
            var attributes = JsonSerializer.Deserialize<List<EntityFieldApiDto>>(EncryptUtil.Decrypt(entityHistoryHashedData.CrmHashedData));

            EntityHistoryApiDto result = new EntityHistoryApiDto
            {
                ConfigurationId = entityHistoryHashedData.ConfigurationId,
                OperationDate = entityHistoryHashedData.OperationDate,
                ExecutionOwnerId = entityHistoryHashedData.ExecutionOwnerId,
                OperationType = entityHistoryHashedData.OperationType,
                LogicalName = entityHistoryHashedData.LogicalName,
                RecordUniqueIdentifier = entityHistoryHashedData.RecordUniqueIdentifier,
                UserId = entityHistoryHashedData.UserId,
                Id = entityHistoryHashedData.Id.ToString(),
                Attributes = attributes
            };

            return result;
        }

    }
}
