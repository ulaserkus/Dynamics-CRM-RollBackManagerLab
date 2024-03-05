using Microsoft.AspNetCore.Http;
using Shared.Api.Abstract;
using Shared.Api.DTOs;
using Shared.Contants;
using Shared.Model.Abstract;
using Shared.Model.Concrete;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Shared.Api.Concrete
{
    public class QueueApiHandler : IQueueApiHandler
    {
        private const string _clientName = AppSecureConstants.QueueApiClientName;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Ignore case
            PropertyNameCaseInsensitive = true // Ignore case
        };
        public QueueApiHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IApiResult> AddEntityHistoryAsync(EntityHistoryApiDto historyApiDto)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AppSecureConstants.IdentityAPICredential);
            var result = await httpClient.PostAsJsonAsync("/api/Queues/AddEntityHistory", historyApiDto);
            var response = await result.Content.ReadAsStringAsync();
            var apiResult = JsonSerializer.Deserialize<ApiResult>(response, _jsonSerializerOptions);
            return apiResult;
        }

        public async Task<IApiResult> DeleteEntityHistoryByConfigurationAsync(Guid configId, string userId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AppSecureConstants.IdentityAPICredential);
            var result = await httpClient.DeleteAsync($"/api/Queues/DeleteEntityHistoryByConfiguration/{configId}?userId={userId}");
            var response = await result.Content.ReadAsStringAsync();

            if (result.StatusCode == HttpStatusCode.NoContent)
            {
                return new ApiResult
                {
                    StatusCode = StatusCodes.Status204NoContent,
                };
            }

            var apiResult = JsonSerializer.Deserialize<ApiResult>(response, _jsonSerializerOptions);
            return apiResult;
        }

        public async Task<IApiResult> DeleteEntityHistoryAsync(string id, string userId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AppSecureConstants.IdentityAPICredential);
            var result = await httpClient.DeleteAsync($"/api/Queues/DeleteEntityHistory/{id}?userId={userId}");
            var response = await result.Content.ReadAsStringAsync();

            if (result.StatusCode == HttpStatusCode.NoContent)
            {
                return new ApiResult
                {
                    StatusCode = StatusCodes.Status204NoContent,
                };
            }

            var apiResult = JsonSerializer.Deserialize<ApiResult>(response, _jsonSerializerOptions);
            return apiResult;
        }

        public async Task<IApiResult> GetEntityHistoryByConfigurationAsync(GetEntityHistoryByConfigurationApiDto historyApiDto)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AppSecureConstants.IdentityAPICredential);
            var result = await httpClient.PostAsJsonAsync("/api/Queues/QueryEntityHistory", historyApiDto);
            var response = await result.Content.ReadAsStringAsync();
            var apiResult = JsonSerializer.Deserialize<ApiResult>(response, _jsonSerializerOptions);
            return apiResult;
        }

        public async Task<IApiResult> GetEntityHistoryWithoutAtrributesByConfigurationAsync(GetEntityHistoryByConfigurationApiDto historyApiDto)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AppSecureConstants.IdentityAPICredential);
            var result = await httpClient.PostAsJsonAsync("/api/Queues/QueryEntityHistoryWithoutAttributes", historyApiDto);
            var response = await result.Content.ReadAsStringAsync();
            var apiResult = JsonSerializer.Deserialize<ApiResult>(response, _jsonSerializerOptions);
            return apiResult;
        }

        public async Task<IApiResult> EntityHistoryAsync(string id, string userId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AppSecureConstants.IdentityAPICredential);
            var result = await httpClient.GetAsync($"/api/Queues/EntityHistory/{id}?userId={userId}");
            var response = await result.Content.ReadAsStringAsync();
            var apiResult = JsonSerializer.Deserialize<ApiResult>(response, _jsonSerializerOptions);
            return apiResult;
        }
    }
}
