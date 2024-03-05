using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using System.Linq;

namespace RollBackManagerLab.CustomFlow.CRM.Handler
{
    public static class PluginTypeHandler
    {
        private const string logicalName = EntityLogicalName.PluginType;

        public static Entity GetPluginTypeByTypeName(string typeName, IOrganizationService service)
        {
            var query = new QueryExpression(logicalName)
            {
                ColumnSet = new ColumnSet(PluginTypeAttribute.AllAttributes),
                Criteria =
                {
                    FilterOperator = LogicalOperator.And,
                    Conditions =
                    {
                        new ConditionExpression(PluginTypeAttribute.TypeName,ConditionOperator.Equal,typeName)
                    }
                },
                TopCount = 1
            };

            var plugins = service.RetrieveMultiple(query).Entities;
            return plugins.FirstOrDefault();
        }
    }
}
