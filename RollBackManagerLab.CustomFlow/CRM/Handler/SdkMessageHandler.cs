using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using System.Linq;

namespace RollBackManagerLab.CustomFlow.CRM.Handler
{
    public static class SdkMessageHandler
    {
        private const string logicalName = EntityLogicalName.SdkMessage;
        public static Entity GetSdkMessageByName(string name, IOrganizationService service)
        {
            var query = new QueryExpression(logicalName)
            {
                ColumnSet = new ColumnSet(SdkMessageAttribute.Name),
                Criteria = new FilterExpression()
                {
                    FilterOperator = LogicalOperator.And,
                    Conditions =
                    {
                       new ConditionExpression(SdkMessageAttribute.Name,ConditionOperator.Equal,name)
                    }
                },
                TopCount = 1
            };

            var result = service.RetrieveMultiple(query).Entities;

            return result.FirstOrDefault();
        }
    }
}
