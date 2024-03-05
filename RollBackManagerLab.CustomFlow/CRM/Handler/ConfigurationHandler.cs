using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using System;
using System.Linq;

namespace RollBackManagerLab.CustomFlow.CRM.Handler
{
    public static class ConfigurationHandler
    {
        private const string logicalName = EntityLogicalName.Configuration;
        public static Entity GetEntityConfiguration(string entityLogicalName, IOrganizationService service)
        {
            var query = new QueryExpression(logicalName)
            {
                ColumnSet = new ColumnSet(ConfigurationAttribute.AllAttributes),
                TopCount = 1,
                Criteria = new FilterExpression(LogicalOperator.And)
                {
                    Conditions =
                   {
                        new ConditionExpression(ConfigurationAttribute.StateCode,ConditionOperator.Equal,ConfigurationStateCodeOptionSet.Active),
                        new ConditionExpression(ConfigurationAttribute.EntityLogicalName,ConditionOperator.Equal,entityLogicalName),
                   }
                }

            };
            return service.RetrieveMultiple(query).Entities.SingleOrDefault();
        }

        public static Entity GetConfigurationById(Guid id, IOrganizationService service)
        {
            return service.Retrieve(logicalName, id, new ColumnSet(ConfigurationAttribute.AllAttributes));
        }

        public static void EmptyUpdateStep(Guid id, IOrganizationService service)
        {
            Entity configurationRecord = new Entity(logicalName, id);
            configurationRecord[ConfigurationAttribute.UpdateStepId] = null;
            service.Update(configurationRecord);
        }

        public static void EmptyDeleteStep(Guid id, IOrganizationService service)
        {
            Entity configurationRecord = new Entity(logicalName, id);
            configurationRecord[ConfigurationAttribute.DeleteStepId] = null;
            service.Update(configurationRecord);
        }
    }
}
