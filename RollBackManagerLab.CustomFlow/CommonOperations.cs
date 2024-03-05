using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using RollBackManagerLab.CustomFlow.API.DTOs;
using RollBackManagerLab.CustomFlow.API.Services;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using RollBackManagerLab.CustomFlow.CRM.Handler;
using RollBackManagerLab.CustomFlow.CRM.Validation;
using RollBackManagerLab.CustomFlow.Extension;
using System;
using System.Linq;
using System.Text;

namespace RollBackManagerLab.CustomFlow
{
    public static class CommonOperations
    {
        public static void OperateAddOrGetFromQueue(IOrganizationService service, IPluginExecutionContext flowContext, ITracingService tracingService)
        {

            StringBuilder logBuilder = new StringBuilder();
            Guid? requestId = null;

            try
            {
                var requestRecord = service.Retrieve(EntityLogicalName.Request, flowContext.PrimaryEntityId, new Microsoft.Xrm.Sdk.Query.ColumnSet(RequestAttribute.Data, RequestAttribute.ApiKeyId, RequestAttribute.Log, RequestAttribute.ConfigurationId, RequestAttribute.RequestType));

                requestId = requestRecord.Id;

                if (requestRecord.Contains(RequestAttribute.Log))
                    logBuilder.AppendLine(requestRecord.GetAttributeValue<string>(RequestAttribute.Log));

                if (requestRecord.Contains(RequestAttribute.Data))
                {
                    if (requestRecord.Contains(RequestAttribute.ApiKeyId))
                    {
                        var apiKeyRef = requestRecord.GetAttributeValue<EntityReference>(RequestAttribute.ApiKeyId);
                        var apiKeyRecord = ApiKeyHandler.GetApiKeyById(apiKeyRef.Id, service);

                        var data = requestRecord.GetAttributeValue<string>(RequestAttribute.Data);
                        var requestType = requestRecord.GetAttributeValue<OptionSetValue>(RequestAttribute.RequestType);
                        var token = GetAuthToken(apiKeyRecord, ref logBuilder, service);

                        if (string.IsNullOrEmpty(token))
                        {
                            logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:ERROR: Occured On Error Token Is Empty !");
                            RequestHandler.UpdateRequestAsFailed(requestRecord.Id, service);
                            return;
                        }

                        if (string.IsNullOrEmpty(data))
                        {
                            logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:ERROR: Occured On Error Data Is Empty !");
                            RequestHandler.UpdateRequestAsFailed(requestRecord.Id, service);
                            return;
                        }

                        if (requestType == null)
                        {
                            logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:ERROR: Occured On Error Request Type Is Empty !");
                            RequestHandler.UpdateRequestAsFailed(requestRecord.Id, service);
                            return;
                        }


                        if (requestType.Value == RequestTypeOptionSet.Backup)
                        {
                            AddToQueue(requestRecord, data, token, ref logBuilder, service);
                        }
                        else if (requestType.Value == RequestTypeOptionSet.Rollback)
                        {
                            GetFromQueue(requestRecord, data, token, ref logBuilder, service);
                        }
                        else
                        {
                            logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:ERROR: Occured On Error When Token Operation !");

                            if (requestId.HasValue)
                                RequestHandler.UpdateRequestAsFailed(requestId.Value, service);
                        }

                    }
                    else
                    {
                        logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:ERROR: Api Key Does Not Exists On Request !");
                        RequestHandler.UpdateRequestAsFailed(requestRecord.Id, service);
                    }
                }
                else
                {
                    logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:ERROR: Request Does Not Contains Data !");
                    RequestHandler.UpdateRequestAsFailed(requestRecord.Id, service);
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace(ex.Message ?? ex.InnerException.Message ?? "Unknown Error!");
                //LOG and Set Fail
                logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}: {ex.Message ?? ex.InnerException.Message ?? "Unknown Error!"}");

                if (requestId.HasValue)
                    RequestHandler.UpdateRequestAsFailed(requestId.Value, service);
            }
            finally
            {
                //Update Log
                if (requestId.HasValue)
                    RequestHandler.UpdateRequestLog(requestId.Value, logBuilder.ToString(), service);

            }
        }

        public static void AddToQueue(Entity requestRecord, string data, string token, ref StringBuilder logBuilder, IOrganizationService service)
        {
            QueueApiHandler.SendToQueue(data, token, ref logBuilder);

            bool deleteStep = false;

            if (requestRecord.Contains(RequestAttribute.ConfigurationId))
            {
                var configurationRef = requestRecord.GetAttributeValue<EntityReference>(RequestAttribute.ConfigurationId);
                var configurationRecord = ConfigurationHandler.GetConfigurationById(configurationRef.Id, service);

                if (configurationRecord.Contains(ConfigurationAttribute.DeleteSucceedRequests))
                {
                    deleteStep = configurationRecord.GetAttributeValue<bool>(ConfigurationAttribute.DeleteSucceedRequests);
                }
            }
            if (deleteStep.Equals(false))
            {
                RequestHandler.UpdateRequestAsSucceed(requestRecord.Id, service);
            }
            else
            {
                RequestHandler.DeleteRequest(requestRecord.Id, service);
            }
        }

        public static void GetFromQueue(Entity requestRecord, string data, string token, ref StringBuilder logBuilder, IOrganizationService service)
        {
            var rollBackRequestData = data.DeserializeFromJson<RollbackDataDto>();

            if (rollBackRequestData == null || string.IsNullOrEmpty(rollBackRequestData.HistoryId))
            {
                logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:ERROR: RollBack Request Data or History Id is empty.");
                RequestHandler.UpdateRequestAsFailed(requestRecord.Id, service);
                return;
            }

            var entityHistory = QueueApiHandler.GetEntityHistory(rollBackRequestData.HistoryId, token);


            if (entityHistory == null || string.IsNullOrEmpty(entityHistory.LogicalName))
            {
                logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:ERROR: RollBack Entity History Data Has Missing Element.");
                RequestHandler.UpdateRequestAsFailed(requestRecord.Id, service);
                return;
            }

            var entity = new Entity(entityHistory.LogicalName, entityHistory.RecordUniqueIdentifier);
            if (entityHistory.Attributes != null && entityHistory.Attributes.Count > 0)
            {
                // entityHistory.Attributes'tan gelen özelliklerin tümü entity.Attributes koleksiyonuna eklenir
                entity.Attributes.AddRange(entityHistory.Attributes.ToDictionary(attr => attr.Name, attr => attr.Value.GetAttributeValueFromString(attr.Type)));
            }

            if (entityHistory.OperationType == TransactionTypeOptionSet.Update)
            {

                logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:INFO: Record Trying Updating. Operation Type : UPDATE");

                RetrieveEntityRequest retrieveEntityRequest = new RetrieveEntityRequest
                {
                    EntityFilters = EntityFilters.Attributes,
                    LogicalName = entityHistory.LogicalName
                };
                RetrieveEntityResponse retrieveOpptyEntityResponse = (RetrieveEntityResponse)service.Execute(retrieveEntityRequest);
                EntityMetadata entityMetadata = retrieveOpptyEntityResponse.EntityMetadata;

                var nullAttributes = entityMetadata.Attributes.Where(metaAttr =>
                                       !entityHistory.Attributes.Exists(histAttr => histAttr.Name.ToUpperInvariant() == metaAttr.LogicalName.ToUpperInvariant()))
                                       .Select(metaAttr => metaAttr.LogicalName)
                                       .ToList(); // nullAttributes'ı bir listeye dönüştür

                // nullAttributes'tan gelen özellikler null değerleriyle birlikte entity.Attributes koleksiyonuna eklenir
                entity.Attributes.AddRange(nullAttributes.ToDictionary(attr => attr, _ => (object)null));

                var updateRequest = new UpdateRequest
                {
                    Target = entity,
                    ["BypassCustomPluginExecution"] = true
                };
                service.Execute(updateRequest);

                logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:INFO: Record RollBack Update Operation Completed.");
            }
            else if (entityHistory.OperationType == TransactionTypeOptionSet.Delete)
            {
                logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:INFO: Record Trying Creating. Operation Type : DELETE");

                var createRequest = new CreateRequest
                {
                    Target = entity,
                    ["BypassCustomPluginExecution"] = true
                };
                var response = (CreateResponse)service.Execute(createRequest);

                logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:INFO: Record RollBack Creation Completed, id: " + response.id.ToString());
            }

            if (IsDeleteRequest(requestRecord, service).Equals(false))
            {
                RequestHandler.UpdateRequestAsSucceed(requestRecord.Id, service);
            }
            else
            {
                RequestHandler.DeleteRequest(requestRecord.Id, service);
            }
        }

        private static bool IsDeleteRequest(Entity requestRecord, IOrganizationService service)
        {
            bool deleteStep = false;

            if (requestRecord.Contains(RequestAttribute.ConfigurationId))
            {
                var configurationRef = requestRecord.GetAttributeValue<EntityReference>(RequestAttribute.ConfigurationId);
                var configurationRecord = ConfigurationHandler.GetConfigurationById(configurationRef.Id, service);

                if (configurationRecord.Contains(ConfigurationAttribute.DeleteSucceedRequests))
                {
                    deleteStep = configurationRecord.GetAttributeValue<bool>(ConfigurationAttribute.DeleteSucceedRequests);
                }
            }

            return deleteStep;
        }

        public static void OperateAddToRequest(IOrganizationService service, IPluginExecutionContext flowContext, ITracingService tracingService)
        {
            StringBuilder logBuilder = new StringBuilder();
            Guid? requestId = null;
            try
            {

                var record = service.Retrieve(flowContext.PrimaryEntityName, flowContext.PrimaryEntityId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                var configurationRecord = ConfigurationHandler.GetEntityConfiguration(flowContext.PrimaryEntityName, service);

                if (configurationRecord == null) return;
                if (!ConfigurationValidation.TryValidate(configurationRecord, record.LogicalName, flowContext.MessageName, flowContext.UserId, service)) return;

                var apiKeyReference = configurationRecord.GetAttributeValue<EntityReference>(ConfigurationAttribute.ApiKeyId);
                var apiKeyRecord = ApiKeyHandler.GetApiKeyById(apiKeyReference.Id, service);
                var data = "";
                if (!ApiKeyValidation.TryValidate(apiKeyRecord))
                {
                    //LOG
                    logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:ERROR: Api Key Validation Error  !");
                }
                else
                {
                    logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:INFO: Validation Completed.");

                    var entityHistory = new EntityHistoryApiDto
                    {

                        ExecutionOwnerId = flowContext.UserId,
                        ConfigurationId = configurationRecord.Id,
                        LogicalName = flowContext.PrimaryEntityName,
                        OperationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffK"),
                        OperationType = flowContext.MessageName.ToUpper() == "DELETE" ? TransactionTypeOptionSet.Delete : TransactionTypeOptionSet.Update,
                        RecordUniqueIdentifier = record.Id
                    };
                    entityHistory.Attributes = record.Attributes.Select(x => new EntityFieldApiDto { Type = x.Value.GetType().AssemblyQualifiedName, Name = x.Key, Value = x.Value.GetAttributeValueAsString() }).ToList();

                    data = entityHistory.SerializeToJson();
                }

                requestId = RequestHandler.CreateBackUpRequest(configurationRecord.Id, apiKeyRecord.Id, data, service);

                logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:INFO: Request Created,{requestId}.");
            }
            catch (Exception ex)
            {
                tracingService.Trace(ex.Message ?? ex.InnerException.Message ?? "Unknown Error!");
                //LOG and Set Fail
                logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:ERROR: {ex.Message ?? ex.InnerException.Message ?? "Unknown Error!"}");

                if (requestId.HasValue)
                    RequestHandler.UpdateRequestAsFailed(requestId.Value, service);
            }
            finally
            {
                //Update Log
                if (requestId.HasValue)
                {
                    logBuilder.AppendLine($"{DateTime.UtcNow.ToLongDateString()}:INFO: Log Is Updating,{requestId}.");

                    RequestHandler.UpdateRequestLog(requestId.Value, logBuilder.ToString(), service);
                }
            }
        }

        public static string GetAuthToken(Entity apiKeyRecord, ref StringBuilder logBuilder, IOrganizationService service, bool tryReadCache = true)
        {
            try
            {

                var apiKey = apiKeyRecord.GetAttributeValue<string>(ApiKeyAttribute.ApiKey);

                if (tryReadCache)
                {
                    var tokenCache = apiKeyRecord.GetAttributeValue<string>(ApiKeyAttribute.TokenCache);
                    var tokenExpireDateUTC = apiKeyRecord.GetAttributeValue<DateTime>(ApiKeyAttribute.TokenExpiresInUTC);
                    if (!string.IsNullOrEmpty(tokenCache) && tokenExpireDateUTC != DateTime.MinValue
                        && tokenExpireDateUTC > DateTime.UtcNow)
                    {
                        return tokenCache;
                    }
                }

                var newToken = IdentityApiHandler.GetAuthToken(apiKey, ref logBuilder);

                if (ApiKeyHandler.ApiKeyRecordIsExists(apiKeyRecord.Id, service))
                    ApiKeyHandler.UpdateToken(apiKeyRecord.Id, newToken, DateTime.UtcNow.AddMinutes(55), service);

                return newToken;
            }
            catch (Exception ex)
            {
                // Hata durumunu logla veya uygun bir şekilde işle
                logBuilder.AppendLine($"Error while obtaining or updating token: {ex.Message}");
                return string.Empty; // veya null veya başka bir değer döndürebilirsiniz
            }
        }
    }
}
