using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using RollBackManagerLab.CustomFlow.CRM.Handler;
using System;
using System.Activities;

namespace RollBackManagerLab.CustomFlow.Flows
{
    public class WhenTransactionTypeChangedUpdateStepFlow : CodeActivity
    {
        [RequiredArgument]
        [Input("Update Step Id")]
        public InArgument<string> UpdateStepId { get; set; }

        [RequiredArgument]
        [Input("Delete Step Id")]
        public InArgument<string> DeleteStepId { get; set; }

        [RequiredArgument]
        [Input("TransactionValues")]
        public InArgument<string> TransactionValues { get; set; }

        [ReferenceTarget(EntityLogicalName.Configuration)]
        [RequiredArgument]
        [Input("RollbackManagerLab.Configuration")]
        public InArgument<EntityReference> ConfigurationRef { get; set; }


        [Output("Success State")]
        public OutArgument<bool> IsSuccessfull { get; set; }

        [Output("Error Message")]
        public OutArgument<string> ErrorMessage { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            IWorkflowContext flowContext = context.GetExtension<IWorkflowContext>();
            ITracingService tracingService = context.GetExtension<ITracingService>();
            IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(flowContext.UserId);

            try
            {
                var values = TransactionValues.Get(context);
                var deleteStepId = DeleteStepId.Get(context);
                var updateStepId = UpdateStepId.Get(context);
                var configurationRef = ConfigurationRef.Get(context);

                //Check Delete Transaction

                if (!values.Contains(TransactionTypeOptionSet.Delete.ToString()) && Guid.TryParse(deleteStepId, out Guid deleteStepRecordId))
                {
                    SdkMessageProcessingHandler.DeleteStep(deleteStepRecordId, service);
                    ConfigurationHandler.EmptyDeleteStep(configurationRef.Id, service);

                }
                else if (values.Contains(TransactionTypeOptionSet.Delete.ToString()) && string.IsNullOrEmpty(deleteStepId))
                {
                    //CREATE
                }


                //Check Update Transaction

                if (!values.Contains(TransactionTypeOptionSet.Update.ToString()) && Guid.TryParse(updateStepId, out Guid updateStepRecordId))
                {

                    SdkMessageProcessingHandler.DeleteStep(updateStepRecordId, service);
                    ConfigurationHandler.EmptyUpdateStep(configurationRef.Id, service);
                }
                else if (values.Contains(TransactionTypeOptionSet.Update.ToString()) && string.IsNullOrEmpty(updateStepId))
                {
                    //CREATE
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
