using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using RollBackManagerLab.CustomFlow.API.Services;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using RollBackManagerLab.CustomFlow.CRM.Handler;
using System;
using System.Activities;
using System.Text;

namespace RollBackManagerLab.CustomFlow.Flows
{
    public class WhenConfigurationDeletedRemoveSdkStep : CodeActivity
    {
        [Output("Success State")]
        public OutArgument<bool> IsSuccessfull { get; set; }

        [Output("Error Message")]
        public OutArgument<string> ErrorMessage { get; set; }



        [RequiredArgument]
        [Input("Update Step Id")]
        public InArgument<string> UpdateStepId { get; set; }

        [RequiredArgument]
        [Input("Delete Step Id")]
        public InArgument<string> DeleteStepId { get; set; }

        [ReferenceTarget(EntityLogicalName.ApiKey)]
        [RequiredArgument]
        [Input("RollbackManagerLab.ApiKey")]
        public InArgument<EntityReference> ApiKeyRef { get; set; }

        [ReferenceTarget(EntityLogicalName.Configuration)]
        [RequiredArgument]
        [Input("RollbackManagerLab.Configuration")]
        public InArgument<EntityReference> ConfigurationRef { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            IWorkflowContext flowContext = context.GetExtension<IWorkflowContext>();
            ITracingService tracingService = context.GetExtension<ITracingService>();
            IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(flowContext.UserId);
            var logBuilder = new StringBuilder();

            // Eylemi oluşturun
            try
            {

                var updateStepId = UpdateStepId.Get<string>(context);
                var deleteStepId = DeleteStepId.Get<string>(context);

                var apiKeyRef = ApiKeyRef.Get<EntityReference>(context);
                var configRef = ConfigurationRef.Get<EntityReference>(context);

                if (Guid.TryParse(updateStepId, out var uid))
                {
                    SdkMessageProcessingHandler.DeleteStep(uid, service);
                }

                if (Guid.TryParse(deleteStepId, out var did))
                {
                    SdkMessageProcessingHandler.DeleteStep(did, service);
                }

                var apiKeyRecord = ApiKeyHandler.GetApiKeyById(apiKeyRef.Id, service);
                var token = CommonOperations.GetAuthToken(apiKeyRecord, ref logBuilder, service);
                QueueApiHandler.DeleteRecordsByConfiguration(configRef.Id, token);

                IsSuccessfull.Set(context, true);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message ?? ex.InnerException.Message ?? "Unknown Error! LOG : " + logBuilder;
                IsSuccessfull.Set(context, false);
                ErrorMessage.Set(context, errorMsg);
                tracingService.Trace(errorMsg);
            }
        }
    }
}
