using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using System;

namespace RollBackManagerLab.CustomFlow.CRM.Handler
{
    public static class ConfigurationUserHandler
    {
        private const string logicalName = EntityLogicalName.ConfigurationUsers;

        public static bool HasActiveConfigurationUser(Guid configurationId, Guid userId, IOrganizationService service)
        {
            var query = new QueryExpression(logicalName)
            {
                ColumnSet = new ColumnSet(ConfigurationUserAttribute.Id),
                Criteria =
                    new FilterExpression(LogicalOperator.And)
                    {
                        Conditions =
                        {
                            new ConditionExpression(ConfigurationUserAttribute.ConfigurationId,ConditionOperator.Equal,configurationId),
                            new ConditionExpression(ConfigurationUserAttribute.UserId,ConditionOperator.Equal,userId),
                            new ConditionExpression(ConfigurationUserAttribute.StateCode,ConditionOperator.Equal,ConfigurationUserStateCodeOptionSet.Active),
                        }
                    },
                TopCount = 1
            };

            var entities = service.RetrieveMultiple(query).Entities;
            return entities.Count > 0;
        }

        public static int ActiveConfigurationUserRecordCount(Guid userId, IOrganizationService service)
        {
            var query = new QueryExpression(logicalName)
            {
                ColumnSet = new ColumnSet(ConfigurationUserAttribute.Id),
                Criteria =
                    new FilterExpression(LogicalOperator.And)
                    {
                        Conditions =
                        {
                            new ConditionExpression(ConfigurationUserAttribute.UserId,ConditionOperator.Equal,userId),
                            new ConditionExpression(ConfigurationUserAttribute.StateCode,ConditionOperator.Equal,ConfigurationUserStateCodeOptionSet.Active),
                        }
                    },
            };

            var entities = service.RetrieveMultiple(query).Entities;
            return entities.Count;
        }
    }
}
