using RollBackManagerLab.CustomFlow.API.Constants;
using RollBackManagerLab.CustomFlow.API.DTOs;
using RollBackManagerLab.CustomFlow.Extension;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RollBackManagerLab.CustomFlow.API.Services
{
    public static class QueueApiHandler
    {
        private static HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(ApiConstant.ApiBaseUrl) };
        public static void SendToQueue(string jsonData, string token, ref StringBuilder logBuilder)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (!string.IsNullOrEmpty(jsonData))
            {
                logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:INFO: Data Is Not Empty.");
            }

            var result = httpClient.PostAsync(ApiConstant.SendQeueuEndpoint, new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json"))
                .GetAwaiter()
                .GetResult();

            result.EnsureSuccessStatusCode();

            logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:INFO: Data Is Sent. Backup Successfull.");
        }

        public static void DeleteRecordsByConfiguration(Guid configurationId, string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = httpClient.DeleteAsync(string.Format(ApiConstant.DeleteRecordsByConfiguration, configurationId))
                .GetAwaiter()
                .GetResult();

            result.EnsureSuccessStatusCode();
        }


        public static EntityHistoryApiDto GetEntityHistory(string historyId, string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = httpClient.GetAsync(string.Format(ApiConstant.HistoryByIdEndpoint, historyId))
                .GetAwaiter()
                .GetResult();

            var response = result.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();

            result.EnsureSuccessStatusCode();

            var responseObject = response.DeserializeFromJson<GetEntityHistoryApiDto>();

            return responseObject.Result;
        }
    }
}
