using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using RollBackManagerLab.CustomFlow.CRM.Handler;
using System;
using System.Activities;

namespace RollBackManagerLab.CustomFlow.Flows
{
    public class WhenConfigurationStateChangedFlow : CodeActivity
    {
        [Output("Success State")]
        public OutArgument<bool> IsSuccessfull { get; set; }

        [Output("Error Message")]
        public OutArgument<string> ErrorMessage { get; set; }


        [RequiredArgument]
        [Input("Step Id")]
        public InArgument<string> StepId { get; set; }

        [AttributeTarget(EntityLogicalName.Configuration, ConfigurationAttribute.StatusCode)]
        [RequiredArgument]
        [Input("StatusCode")]
        public InArgument<OptionSetValue> StatusCode { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            IWorkflowContext flowContext = context.GetExtension<IWorkflowContext>();
            ITracingService tracingService = context.GetExtension<ITracingService>();
            IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(flowContext.UserId);

            // Eylemi oluşturun
            try
            {

                var stepId = StepId.Get<string>(context);
                var statusCode = StatusCode.Get<OptionSetValue>(context);

                if (Guid.TryParse(stepId, out var id))
                {
                    if (statusCode.Value == SdkMessageProcessingStatusCode.Disabled)
                    {
                        SdkMessageProcessingHandler.DisableStep(id, service);
                    }
                    else if (statusCode.Value == SdkMessageProcessingStatusCode.Enabled)
                    {
                        SdkMessageProcessingHandler.EnableStep(id, service);
                    }
                }

                IsSuccessfull.Set(context, true);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message ?? ex.InnerException.Message ?? "Unknown Error!";
                IsSuccessfull.Set(context, false);
                ErrorMessage.Set(context, errorMsg);
                tracingService.Trace(errorMsg);
            }
        }
    }
}
