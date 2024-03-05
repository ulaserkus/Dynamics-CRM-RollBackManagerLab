using RollBackManagerLab.CustomFlow.API.Constants;
using RollBackManagerLab.CustomFlow.API.DTOs;
using RollBackManagerLab.CustomFlow.Extension;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RollBackManagerLab.CustomFlow.API.Services
{
    public static class IdentityApiHandler
    {
        private static HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(ApiConstant.ApiBaseUrl) };
        public static string GetAuthToken(string apiKey, ref StringBuilder logBuilder)
        {

            httpClient.DefaultRequestHeaders.Add("ApiKey", apiKey);

            var result = httpClient.PostAsync(ApiConstant.AuthenticationEndpoint, null)
                .GetAwaiter()
                .GetResult();

            httpClient.DefaultRequestHeaders.Remove("ApiKey");

            result.EnsureSuccessStatusCode();

            var response = result.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();

            if (!string.IsNullOrEmpty(response))
            {
                var apiResult = response.DeserializeFromJson<LoginApiResultDto>();

                logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:INFO: Authentication Succeed.");

                return apiResult.Result;
            }

            return string.Empty;
        }


        public static List<ApiKeyDto> GetApiKeys(string token, ref StringBuilder logBuilder)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = httpClient.PostAsync(ApiConstant.GetUserApiKeys, null)
                .GetAwaiter()
                .GetResult();

            result.EnsureSuccessStatusCode();

            var response = result.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();

            if (!string.IsNullOrEmpty(response))
            {
                var apiResult = response.DeserializeFromJson<GetUserApiKeyResultsDto>();

                logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:INFO: Authentication Succeed.");

                return apiResult.Result;
            }

            return null;
        }
    }
}
