using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using RollBackManagerLab.CustomFlow.CRM.Handler;
using System;
using System.Activities;

namespace RollBackManagerLab.CustomFlow.Flows
{
    public class WhenConfigurationCreatedAddSdkStep : CodeActivity
    {
        [Output("Success State")]
        public OutArgument<bool> IsSuccessfull { get; set; }

        [Output("Error Message")]
        public OutArgument<string> ErrorMessage { get; set; }

        [Output("Step Id")]
        public OutArgument<string> StepId { get; set; }


        [RequiredArgument]
        [Input("Message")]
        public InArgument<string> Message { get; set; }

        [RequiredArgument]
        [Input("PluginName")]
        public InArgument<string> PluginName { get; set; }

        [RequiredArgument]
        [Input("Stage")]
        public InArgument<int> Stage { get; set; }

        [RequiredArgument]
        [Input("Mode")]
        public InArgument<int> Mode { get; set; }

        [RequiredArgument]
        [Input("LogicalName")]
        public InArgument<string> LogicalName { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            IWorkflowContext flowContext = context.GetExtension<IWorkflowContext>();
            ITracingService tracingService = context.GetExtension<ITracingService>();
            IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(flowContext.UserId);

            // Eylemi oluşturun
            try
            {

                var message = Message.Get(context);
                var stage = Stage.Get(context);
                var mode = Mode.Get(context);
                var primaryObjectCode = LogicalName.Get(context);
                var pluginName = PluginName.Get(context);

                var pluginType = PluginTypeHandler.GetPluginTypeByTypeName(pluginName, service);

                if (pluginType != null && pluginType.Id != Guid.Empty)
                {
                    var sdkMessage = SdkMessageHandler.GetSdkMessageByName(message, service);

                    if (sdkMessage != null && sdkMessage.Id != Guid.Empty)
                    {
                        var filter = SdkMessageFilterHandler.GetSdkMessageFilterByPrimaryObjectAndSdkMessage(primaryObjectCode, sdkMessage.Id, service);

                        if (filter != null && filter.Id != Guid.Empty)
                        {
                            var stepId = SdkMessageProcessingHandler.CreateSdkMessageProcessingStep(filter.Id, sdkMessage.Id, pluginType.Id, pluginName, message, primaryObjectCode, mode, stage, service);
                            IsSuccessfull.Set(context, true); StepId.Set(context, stepId.ToString());


                        }
                        else
                        {
                            IsSuccessfull.Set(context, false);
                            ErrorMessage.Set(context, "SDK Message Filter Record Not Found !");
                        }
                    }
                    else
                    {
                        IsSuccessfull.Set(context, false);
                        ErrorMessage.Set(context, "SDK Message Record Not Found !");
                    }
                }
                else
                {
                    IsSuccessfull.Set(context, false);
                    ErrorMessage.Set(context, "Related Plugin Record Not Found !");
                }
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
