using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using System;

namespace RollBackManagerLab.CustomFlow.CRM.Handler
{
    public static class SystemUserRoleHandler
    {
        public const string logicalName = EntityLogicalName.SystemUserRoles;

        public static bool UserHasRelatedRole(Guid roleId, Guid userId, IOrganizationService service)
        {
            var query = new QueryExpression(EntityLogicalName.SystemUserRoles)
            {
                ColumnSet = new ColumnSet(SystemUserRoleAttribute.AllAttributes),
                Criteria =
                {
                    FilterOperator = LogicalOperator.And,
                    Conditions =
                    {
                       new ConditionExpression(SystemUserRoleAttribute.SystemUserId,ConditionOperator.Equal,userId),
                       new ConditionExpression(SystemUserRoleAttribute.RoleId,ConditionOperator.Equal,roleId),
                    }
                },
                TopCount = 1
            };

            var result = service.RetrieveMultiple(query).Entities;

            return result.Count > 0;

        }

    }
}
