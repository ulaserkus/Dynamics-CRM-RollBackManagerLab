namespace RollBackManagerLab.CustomFlow.CRM.Constants
{
    public static class ApiKeyAttribute
    {
        public const string Id = "cr70c_rollbackmanagerapikeyid";
        public const string ApiKey = "cr70c_apikey";
        public const string ExpiresInUTC = "cr70c_expiresinutc";
        public const string StateCode = "statecode";
        public const string TokenCache = "cr70c_tokencache";
        public const string TokenExpiresInUTC = "cr70c_tokenexpritiondate";
        public readonly static string[] AllAttributes = new string[]
        {
            Id,ApiKey,ExpiresInUTC,StateCode,TokenCache,TokenExpiresInUTC
        };
    }

    public static class ConfigurationAttribute
    {
        public const string Id = "cr70c_rollbackmanagerlabconfigurationid";
        public const string EntityLogicalName = "cr70c_entitylogicalname";
        public const string TransactionType = "cr70c_transactiontype";
        public const string ApiKeyId = "cr70c_rollbackmanagerlabapikey";
        public const string StateCode = "statecode";
        public const string StatusCode = "statuscode";
        public const string DeleteSucceedRequests = "cr70c_deletesucceedrequests";
        public const string UpdateStepId = "cr70c_updatestepid";
        public const string DeleteStepId = "cr70c_deletestepid";
        public readonly static string[] AllAttributes = new string[]
        {
            Id,EntityLogicalName,TransactionType,ApiKeyId,StateCode,DeleteSucceedRequests
        };
    }

    public static class ConfigurationUserAttribute
    {
        public const string Id = "cr70c_rollbackmanagerlabconfigurationuserid";
        public const string StateCode = "statecode";
        public const string ConfigurationId = "cr70c_configurationid";
        public const string UserId = "cr70c_userid";
        public readonly static string[] AllAttributes = new string[]
        {
            Id,StateCode,ConfigurationId,UserId
        };
    }

    public static class RequestAttribute
    {
        public const string Id = "cr70c_rollbackmanagerlabrequestid";
        public const string StateCode = "statecode";
        public const string RequestNumber = "cr70c_requestnumber";
        public const string RequestType = "cr70c_requesttype";
        public const string Data = "cr70c_data";
        public const string StatusCode = "statuscode";
        public const string Log = "cr70c_log";
        public const string ConfigurationId = "cr70c_configurationid";
        public const string ApiKeyId = "cr70c_apikeyid";
        public readonly static string[] AllAttributes = new string[]
        {
            Id,StateCode,RequestNumber,Data,RequestType,StatusCode,Log,ConfigurationId,ApiKeyId
        };
    }

    public static class SdkMessageAttribute
    {
        public const string Id = "sdkmessageid";
        public const string Name = "name";
        public readonly static string[] AllAttributes = new string[]
        {
            Id,Name
        };
    }

    public static class SdkMessageFilterAttribute
    {
        public const string Id = "sdkmessagefilterid";
        public const string SdkMessageId = "sdkmessageid";
        public const string PrimaryObjectTypeCdde = "primaryobjecttypecode";
        public readonly static string[] AllAttributes = new string[]
        {
            Id,SdkMessageId,PrimaryObjectTypeCdde
        };
    }

    public static class PluginTypeAttribute
    {
        public const string Id = "plugintypeid";
        public const string TypeName = "typename";
        public readonly static string[] AllAttributes = new string[]
        {
            Id,TypeName
        };
    }

    public static class SdkMessageProcessingStepAttribute
    {
        public const string Id = "sdkmessageprocessingstepid";
        public const string Name = "name";
        public const string SdkMessageId = "sdkmessageid";
        public const string EventHandlerId = "eventhandler";
        public const string SupportedDeployment = "supporteddeployment";
        public const string Mode = "mode";
        public const string Stage = "stage";
        public const string StatusCode = "statuscode";
        public const string StateCode = "statecode";
        public const string AsyncAutoDelete = "asyncautodelete";
        public const string Rank = "rank";
        public const string Description = "description";
        public const string SdkMessageFilterId = "sdkmessagefilterid";
        public readonly static string[] AllAttributes = new string[]
        {
            Id,Name,SdkMessageId,EventHandlerId, SupportedDeployment, Mode,Stage, StatusCode, StateCode, AsyncAutoDelete, Rank, Description,SdkMessageFilterId
        };

    }


    public static class SystemUserAttribute
    {
        public const string Id = "systemuserid";
        public readonly static string[] AllAttributes = new string[]
        {
            Id
        };
    }

    public static class RoleAttributes
    {
        public const string Id = "roleid";
        public const string Name = "name";
        public readonly static string[] AllAttributes = new string[]
        {
            Id
        };
    }

    public static class SystemUserRoleAttribute
    {
        public const string Id = "systemuserroleid";
        public const string RoleId = "roleid";
        public const string SystemUserId = "systemuserid";
        public readonly static string[] AllAttributes = new string[]
        {
            Id,RoleId,SystemUserId
        };
    }
}
