namespace RollBackManagerLab.CustomFlow.API.Constants
{
    public static class ApiConstant
    {
        public const string ApiBaseUrl = "https://apiurl.com/";
        public const string SendQeueuEndpoint = "/queue/add";
        public const string AuthenticationEndpoint = "/identity/loginwithkey";
        public const string GetUserApiKeys = "/identity/userkeys";
        public const string DeleteRecordsByConfiguration = "/queue/deletebyconfiguration/{0}";
        public const string HistoryByIdEndpoint = "/queue/history/{0}";
    }
}
