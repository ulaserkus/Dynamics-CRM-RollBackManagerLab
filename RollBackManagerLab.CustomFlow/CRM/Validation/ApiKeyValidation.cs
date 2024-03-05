using Microsoft.Xrm.Sdk;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using System;
using System.Linq;

namespace RollBackManagerLab.CustomFlow.CRM.Validation
{
    public class ApiKeyValidation
    {
        protected ApiKeyValidation() { }
        public static bool TryValidate(Entity apiKeyRecord)
        {
            if (!apiKeyRecord.Attributes.Keys.ToList().TrueForAll(x => ApiKeyAttribute.AllAttributes.Contains(x)))
                return false;

            if (!apiKeyRecord.Contains(ApiKeyAttribute.ExpiresInUTC))
                return false;

            var expireDate = apiKeyRecord.GetAttributeValue<DateTime>(ApiKeyAttribute.ExpiresInUTC);

            if (expireDate < DateTime.UtcNow)
                return false;

            return true;
        }
    }
}
