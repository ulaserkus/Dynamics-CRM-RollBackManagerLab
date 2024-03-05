using Queue.Framework.Data.Model;
using Shared.Api.DTOs;

namespace Queue.Framework.Core.Mapper.Abstract
{
    public interface IEntityHistoryApiDtoMapper
    {
        EntityHistoryApiDto ResolveHashedEntityHistory(EntityHistoryHashedData entityHistoryHashedData);
        EntityHistoryApiDto MapWithoutAttributesFromHashedData(EntityHistoryHashedData entityHistoryHashedData);
    }
}
