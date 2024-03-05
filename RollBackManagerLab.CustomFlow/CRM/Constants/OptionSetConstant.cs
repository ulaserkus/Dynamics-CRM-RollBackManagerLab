namespace RollBackManagerLab.CustomFlow.CRM.Constants
{
    public static class RequestTypeOptionSet
    {
        public const int Backup = 1;
        public const int Rollback = 2;
    }

    public static class SdkMessageProcessingStatusCode
    {
        public const int Enabled = 1;
        public const int Disabled = 2;
    }

    public static class SdkMessageProcessingStateCode
    {
        public const int Enabled = 0;
        public const int Disabled = 1;
    }

    public static class RequestStatusCodeOptionSet
    {
        public const int Failed = 287580001;
        public const int Succeed = 1;
        public const int OnProcess = 287580002;
    }

    public static class RequestStateCodeOptionSet
    {
        public const int Active = 0;
        public const int DeActive = 1;
    }

    public static class ConfigurationStateCodeOptionSet
    {
        public const int Active = 0;
        public const int DeActive = 1;
    }

    public static class ConfigurationUserStateCodeOptionSet
    {
        public const int Active = 0;
        public const int DeActive = 1;
    }

    public static class ApiKeyStateCodeOptionSet
    {
        public const int Active = 0;
        public const int DeActive = 1;
    }

    public static class TransactionTypeOptionSet
    {
        public const int Delete = 1;
        public const int Update = 2;
    }
}
