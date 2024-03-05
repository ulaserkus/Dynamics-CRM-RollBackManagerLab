using Microsoft.Xrm.Sdk;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using System;

namespace RollBackManagerLab.CustomFlow.CRM.Handler
{
    public static class SdkMessageProcessingHandler
    {
        private const string logicalName = EntityLogicalName.SdkMessageProcessingStep;
        public static Guid CreateSdkMessageProcessingStep(Guid filterId, Guid sdkMessageId, Guid pluginTypeId, string name, string message, string primaryObjectTypeCode, int mode, int stage, IOrganizationService service)
        {
            Entity sdkMessageProcessingStep = new Entity(logicalName);
            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.Name] = $"{name}_{message}_of_{primaryObjectTypeCode}_V1";
            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.SdkMessageId] = new EntityReference(EntityLogicalName.SdkMessage, sdkMessageId);
            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.EventHandlerId] = new EntityReference(EntityLogicalName.PluginType, pluginTypeId);
            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.SupportedDeployment] = new OptionSetValue(0);
            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.Mode] = new OptionSetValue(mode);
            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.Stage] = new OptionSetValue(stage);

            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.StatusCode] = new OptionSetValue(SdkMessageProcessingStatusCode.Enabled); // Enabled
            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.StateCode] = new OptionSetValue(SdkMessageProcessingStateCode.Enabled); // Enabled

            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.AsyncAutoDelete] = mode == 1;
            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.Rank] = 1;
            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.Description] = $"{name}_{message}_of_{primaryObjectTypeCode}_V1";
            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.SdkMessageFilterId] = new EntityReference(EntityLogicalName.SdkMessageFilter, filterId);
            return service.Create(sdkMessageProcessingStep);
        }

        public static void DisableStep(Guid stepId, IOrganizationService service)
        {
            Entity sdkMessageProcessingStep = new Entity(logicalName, stepId);
            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.StatusCode] = new OptionSetValue(SdkMessageProcessingStatusCode.Disabled); // Disabled
            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.StateCode] = new OptionSetValue(SdkMessageProcessingStateCode.Disabled); // Disabled
            service.Update(sdkMessageProcessingStep);
        }

        public static void EnableStep(Guid stepId, IOrganizationService service)
        {
            Entity sdkMessageProcessingStep = new Entity(logicalName, stepId);
            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.StatusCode] = new OptionSetValue(SdkMessageProcessingStatusCode.Enabled); // Enabled
            sdkMessageProcessingStep[SdkMessageProcessingStepAttribute.StateCode] = new OptionSetValue(SdkMessageProcessingStateCode.Enabled); // Enabled
            service.Update(sdkMessageProcessingStep);
        }

        public static void DeleteStep(Guid stepId, IOrganizationService service)
        {
            service.Delete(logicalName, stepId);
        }
    }
}
