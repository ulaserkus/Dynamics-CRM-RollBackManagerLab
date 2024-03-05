using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Queue.Framework.Core.Services.Abstract;
using Shared.Api.DTOs;

namespace Queue.Framework.Core.Services.Concrete
{
    public class EntityHistoryDtoSub : QueueSub<EntityHistoryApiDto>, IEntityHistoryDtoSub
    {
        private readonly IEntityHistoryHashedDataService _entityHistoryService;
        public EntityHistoryDtoSub(IBusRegistrationContext context)
        {
            using var scope = context.CreateScope();
            var entityHistoryService = scope.ServiceProvider.GetRequiredService<IEntityHistoryHashedDataService>();
            _entityHistoryService = entityHistoryService;
        }
        public override async Task Consume(ConsumeContext<EntityHistoryApiDto> context)
        {
            var entityHistory = context.Message;
            await _entityHistoryService.InsertEntitHistoryAsync(entityHistory);
        }
    }
}
