using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using System;
using System.Linq;

namespace RollBackManagerLab.CustomFlow.CRM.Handler
{
    public static class SdkMessageFilterHandler
    {
        private const string logicalName = EntityLogicalName.SdkMessageFilter;

        public static Entity GetSdkMessageFilterByPrimaryObjectAndSdkMessage(string primaryObjectCode, Guid sdkMessageId, IOrganizationService service)
        {

            QueryExpression queryExpression = new QueryExpression(logicalName)
            {
                ColumnSet = new ColumnSet(SdkMessageFilterAttribute.AllAttributes),
                Criteria = new FilterExpression
                {
                    FilterOperator = LogicalOperator.And,
                    Conditions =
                    {
                                    new ConditionExpression(SdkMessageFilterAttribute.PrimaryObjectTypeCdde,ConditionOperator.Equal,primaryObjectCode),
                                    new ConditionExpression(SdkMessageFilterAttribute.SdkMessageId,ConditionOperator.Equal,sdkMessageId)
                                },
                },
                TopCount = 1,
            };

            var filters = service.RetrieveMultiple(queryExpression).Entities;
            return filters.FirstOrDefault();
        }
    }
}
