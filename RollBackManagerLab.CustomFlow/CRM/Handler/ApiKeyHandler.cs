using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using System;

namespace RollBackManagerLab.CustomFlow.CRM.Handler
{
    public static class ApiKeyHandler
    {
        private const string logicalName = EntityLogicalName.ApiKey;
        public static Entity GetApiKeyById(Guid id, IOrganizationService service)
        {
            return service.Retrieve(logicalName, id, new ColumnSet(ApiKeyAttribute.AllAttributes));
        }

        public static bool ApiKeyRecordIsExists(Guid id, IOrganizationService service)
        {
            var query = new QueryExpression(logicalName)
            {
                ColumnSet = new ColumnSet(ApiKeyAttribute.Id),
                Criteria =
                {
                    FilterOperator = LogicalOperator.And,
                    Conditions =
                    {
                        new ConditionExpression(ApiKeyAttribute.Id,ConditionOperator.Equal,id)
                    }
                },
                TopCount = 1,
            };

            var entities = service.RetrieveMultiple(query).Entities;
            return entities.Count > 0;
        }

        public static bool IsExistSameApiKey(string key, IOrganizationService service)
        {
            var query = new QueryExpression(logicalName)
            {
                ColumnSet = new ColumnSet(ApiKeyAttribute.Id),
                Criteria =
                {
                    FilterOperator = LogicalOperator.And,
                    Conditions =
                    {
                        new ConditionExpression(ApiKeyAttribute.ApiKey,ConditionOperator.Equal,key)
                    }
                },
                TopCount = 1,
            };

            var entities = service.RetrieveMultiple(query).Entities;
            return entities.Count > 0;
        }

        public static void UpdateToken(Guid apiKeyId, string token, DateTime expireDate, IOrganizationService service)
        {
            var apiKeyRecord = new Entity(logicalName, apiKeyId);
            apiKeyRecord[ApiKeyAttribute.TokenCache] = token;
            apiKeyRecord[ApiKeyAttribute.TokenExpiresInUTC] = expireDate;
            service.Update(apiKeyRecord);
        }

        public static void UpdateApiKeyExpiration(Guid apiKeyId, DateTime expireDate, IOrganizationService service)
        {
            var apiKeyRecord = new Entity(logicalName, apiKeyId);
            apiKeyRecord[ApiKeyAttribute.ExpiresInUTC] = expireDate;
            service.Update(apiKeyRecord);
        }
    }
}
