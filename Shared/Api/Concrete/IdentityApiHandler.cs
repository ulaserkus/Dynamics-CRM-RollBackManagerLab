using Shared.Api.Abstract;
using Shared.Api.DTOs;
using Shared.Contants;
using Shared.Model.Abstract;
using Shared.Model.Concrete;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Shared.Api.Concrete
{
    public class IdentityApiHandler : IIdentityApiHandler
    {
        private const string _clientName = AppSecureConstants.IdentityApiClientName;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Ignore case
            PropertyNameCaseInsensitive = true // Ignore case
        };
        public IdentityApiHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IApiResult> AddUserAsync(AddUserRequestApiDto addUserRequest)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{addUserRequest.AppKey}:{addUserRequest.AppSecret}"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            var result = await httpClient.PostAsJsonAsync("/api/users/add", addUserRequest);
            var response = await result.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(response))
            {
                return new ApiResult
                {
                    ErrorCode = (int)result.StatusCode,
                    ErrorMessage = result.ReasonPhrase ?? "",
                    StatusCode = (int)result.StatusCode,
                };
            }
            var apiResult = JsonSerializer.Deserialize<ApiResult>(response, _jsonSerializerOptions);
            return apiResult;
        }

        public async Task<IApiResult> AddUserWithAdminServiceAsync(AddUserRequestApiDto addUserRequest)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AppSecureConstants.IdentityAPICredential);
            var result = await httpClient.PostAsJsonAsync("/api/users/add", addUserRequest);
            var response = await result.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(response))
            {
                return new ApiResult
                {
                    ErrorCode = (int)result.StatusCode,
                    ErrorMessage = result.ReasonPhrase ?? "",
                    StatusCode = (int)result.StatusCode,
                };
            }
            var apiResult = JsonSerializer.Deserialize<ApiResult>(response, _jsonSerializerOptions);
            return apiResult;
        }
        public async Task<IApiResult> LoginAsync(LoginApiDto loginDto)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AppSecureConstants.IdentityAPICredential);
            var result = await httpClient.PostAsJsonAsync("/api/users/login", loginDto);
            var response = await result.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(response))
            {
                return new ApiResult
                {
                    ErrorCode = (int)result.StatusCode,
                    ErrorMessage = "",
                    StatusCode = (int)result.StatusCode,
                };
            }
            var apiResult = JsonSerializer.Deserialize<ApiResult>(response, _jsonSerializerOptions);
            return apiResult;
        }

        public async Task<IApiResult> LoginWithKeyAsync(string apiKey)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AppSecureConstants.IdentityAPICredential);
            httpClient.DefaultRequestHeaders.Add("ApiKey", apiKey);

            var result = await httpClient.PostAsync($"/api/users/LoginWithKey", null);

            var response = await result.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(response))
            {
                return new ApiResult
                {
                    ErrorCode = (int)result.StatusCode,
                    ErrorMessage = "",
                    StatusCode = (int)result.StatusCode,
                };
            }
            var apiResult = JsonSerializer.Deserialize<ApiResult>(response, _jsonSerializerOptions);
            return apiResult;
        }

        public async Task<IApiResult> GetUserKeysByIdAsync(string userId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AppSecureConstants.IdentityAPICredential);

            var result = await httpClient.PostAsync($"/api/users/UserKeys/{userId}", null);

            var response = await result.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(response))
            {
                return new ApiResult
                {
                    ErrorCode = (int)result.StatusCode,
                    ErrorMessage = "",
                    StatusCode = (int)result.StatusCode,
                };
            }
            var apiResult = JsonSerializer.Deserialize<ApiResult>(response, _jsonSerializerOptions);
            return apiResult;
        }
    }
}
