using Microsoft.Xrm.Sdk;
using System;
using System.Threading;

namespace RollBackManagerLab.CustomFlow.Plugins
{
    public class WhenRequestCreatedPostAsyncPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext flowContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(flowContext.UserId);

            //Try Send To BackupManager
            try
            {
                Thread.Sleep(3000);

                CommonOperations.OperateAddOrGetFromQueue(service, flowContext, tracingService);
            }
            catch (Exception ex)
            {
                tracingService.Trace(ex.Message ?? ex.InnerException.Message ?? "Unknown Error!");
            }
        }
    }
}
