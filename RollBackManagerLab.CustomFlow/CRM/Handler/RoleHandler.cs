using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using System.Linq;

namespace RollBackManagerLab.CustomFlow.CRM.Handler
{
    public static class RoleHandler
    {
        private const string logicalName = EntityLogicalName.Role;

        public static Entity GetRoleByName(string roleName, string[] columns, IOrganizationService service)
        {
            var query = new QueryExpression(logicalName)
            {
                ColumnSet = new ColumnSet(columns),
                Criteria =
                {
                    FilterOperator = LogicalOperator.And,
                    Conditions =
                    {
                        new ConditionExpression(RoleAttributes.Name,ConditionOperator.Equal,roleName)
                    }
                },
                TopCount = 1,

            };
            var result = service.RetrieveMultiple(query).Entities;
            return result.FirstOrDefault();
        }

    }
}
