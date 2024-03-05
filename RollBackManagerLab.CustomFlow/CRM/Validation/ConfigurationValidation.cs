using Microsoft.Xrm.Sdk;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using RollBackManagerLab.CustomFlow.CRM.Handler;
using System;
using System.Linq;

namespace RollBackManagerLab.CustomFlow.CRM.Validation
{
    public class ConfigurationValidation
    {
        protected ConfigurationValidation() { }
        public static bool TryValidate(Entity configurationRecord, string primaryEntityName, string operationType, Guid userId, IOrganizationService service)
        {
            if (!configurationRecord.Attributes.Keys.ToList().TrueForAll(x => ConfigurationAttribute.AllAttributes.Contains(x)))
                return false;

            if (!primaryEntityName.Equals(configurationRecord.GetAttributeValue<string>(ConfigurationAttribute.EntityLogicalName)))
                return false;

            if (!ConfigurationUserHandler.HasActiveConfigurationUser(configurationRecord.Id, userId, service))
                return false;

            var transactionTypes = configurationRecord.GetAttributeValue<OptionSetValueCollection>(ConfigurationAttribute.TransactionType);
            var transactionTypeValues = transactionTypes.Select(x => x.Value).ToList();

            if (operationType.ToUpperInvariant() == "DELETE" && transactionTypeValues.Contains(TransactionTypeOptionSet.Delete))
            {
                return true;
            }
            else if (operationType.ToUpperInvariant() == "UPDATE" && transactionTypeValues.Contains(TransactionTypeOptionSet.Update))
            {
                return true;
            }

            return false;
        }
    }
}
