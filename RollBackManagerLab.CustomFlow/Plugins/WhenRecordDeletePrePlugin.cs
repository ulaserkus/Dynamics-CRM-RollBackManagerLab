using Microsoft.Xrm.Sdk;
using System;

namespace RollBackManagerLab.CustomFlow.Plugins
{
    public sealed class WhenRecordDeletePrePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext flowContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(flowContext.UserId);

            //Create Request Entity

            try
            {
                CommonOperations.OperateAddToRequest(service, flowContext, tracingService);
            }
            catch (Exception ex)
            {
                tracingService.Trace(ex.Message ?? ex.InnerException.Message ?? "Unknown Error!");
            }
        }
    }
}
