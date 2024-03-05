using Microsoft.AspNetCore.Mvc;
using Queue.Framework.Core.Services.Abstract;
using Shared.Api.DTOs;
using Shared.CustomController;

namespace Queue.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueuesController : CustomBaseController
    {
        private readonly IEntityHistoryDtoPub _historyDtoPub;
        private readonly IEntityHistoryHashedDataService _entityHistoryService;
        public QueuesController(IEntityHistoryDtoPub historyDtoPub, IEntityHistoryHashedDataService entityHistoryService)
        {
            _historyDtoPub = historyDtoPub;
            _entityHistoryService = entityHistoryService;
        }

        [HttpGet("EntityHistory/{id}")]
        public async Task<IActionResult> EntityHistoryAsync([FromRoute] string id, string userId)
        {
            var result = await _entityHistoryService.GetEntityHistoryAsync(id, userId);
            return CreateActionResultInstance(result);
        }

        [HttpPost("AddEntityHistory")]
        public async Task<IActionResult> AddEntityHistoryAsync([FromBody] EntityHistoryApiDto entityHistoryDto)
        {
            var result = await _historyDtoPub.AddDataToQueueReturnApiResultAsync(entityHistoryDto);
            return CreateActionResultInstance(result);
        }

        [HttpPost("QueryEntityHistory")]
        public async Task<IActionResult> QueryEntityHistoryAsync([FromBody] GetEntityHistoryByConfigurationApiDto historyByConfigurationApiDto)
        {
            var result = await _entityHistoryService.GetEntityHistoryByConfigurationAsync(historyByConfigurationApiDto);
            return CreateActionResultInstance(result);
        }

        [HttpPost("QueryEntityHistoryWithoutAttributes")]
        public async Task<IActionResult> QueryEntityHistoryWithoutAttributesAsync([FromBody] GetEntityHistoryByConfigurationApiDto historyByConfigurationApiDto)
        {
            var result = await _entityHistoryService.GetEntityHistoryWithoutAttributesByConfigurationAsync(historyByConfigurationApiDto);
            return CreateActionResultInstance(result);
        }

        [HttpDelete("DeleteEntityHistoryByConfiguration/{configurationId}")]
        public async Task<IActionResult> DeleteEntityHistoryByConfigurationAsync([FromRoute] Guid configurationId, string userId)
        {
            var result = await _entityHistoryService.DeleteEntityHistoryByConfigurationAsync(configurationId, userId);
            return CreateActionResultInstance(result);
        }

        [HttpDelete("DeleteEntityHistory/{id}")]
        public async Task<IActionResult> DeleteEntityHistoryAsync([FromRoute] string id, string userId)
        {
            var result = await _entityHistoryService.DeleteEntityHistoryAsync(id, userId);
            return CreateActionResultInstance(result);
        }
    }
}
