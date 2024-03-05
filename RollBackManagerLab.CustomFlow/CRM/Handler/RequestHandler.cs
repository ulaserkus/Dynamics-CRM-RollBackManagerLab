using Microsoft.Xrm.Sdk;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using System;

namespace RollBackManagerLab.CustomFlow.CRM.Handler
{
    public static class RequestHandler
    {
        private const string logicalName = EntityLogicalName.Request;
        public static Guid CreateBackUpRequest(Guid configId, Guid apiKeyId, string data, IOrganizationService service)
        {
            Entity newRecord = new Entity(logicalName);
            newRecord[RequestAttribute.RequestType] = new OptionSetValue(RequestTypeOptionSet.Backup);
            newRecord[RequestAttribute.ConfigurationId] = new EntityReference(EntityLogicalName.Configuration, configId);
            newRecord[RequestAttribute.ApiKeyId] = new EntityReference(EntityLogicalName.ApiKey, apiKeyId);
            newRecord[RequestAttribute.Data] = data;
            return service.Create(newRecord);
        }

        public static void UpdateRequestAsSucceed(Guid id, IOrganizationService service)
        {
            Entity newRecord = new Entity(logicalName, id);
            newRecord[RequestAttribute.StatusCode] = new OptionSetValue(RequestStatusCodeOptionSet.Succeed);
            newRecord[RequestAttribute.StateCode] = new OptionSetValue(RequestStateCodeOptionSet.Active);
            service.Update(newRecord);
        }


        public static void UpdateRequestAsFailed(Guid id, IOrganizationService service)
        {
            Entity newRecord = new Entity(logicalName, id);
            newRecord[RequestAttribute.StatusCode] = new OptionSetValue(RequestStatusCodeOptionSet.Failed);
            newRecord[RequestAttribute.StateCode] = new OptionSetValue(RequestStateCodeOptionSet.Active);
            service.Update(newRecord);
        }

        public static void UpdateRequestLog(Guid id, string log, IOrganizationService service)
        {
            Entity newRecord = new Entity(logicalName, id);
            newRecord[RequestAttribute.Log] = log;
            service.Update(newRecord);
        }

        public static void UpdateRequestData(Guid id, string data, IOrganizationService service)
        {
            Entity newRecord = new Entity(logicalName, id);
            newRecord[RequestAttribute.Data] = data;
            service.Update(newRecord);
        }

        public static void UpdateRequestConfigurationAndApiKey(Guid id, Guid configId, Guid apiKeyId, IOrganizationService service)
        {
            Entity newRecord = new Entity(logicalName, id);
            newRecord[RequestAttribute.ConfigurationId] = new EntityReference(EntityLogicalName.Configuration, configId);
            newRecord[RequestAttribute.ApiKeyId] = new EntityReference(EntityLogicalName.Configuration, apiKeyId);
            service.Update(newRecord);
        }
        public static void DeleteRequest(Guid id, IOrganizationService service)
        {
            service.Delete(logicalName, id);
        }
    }
}
