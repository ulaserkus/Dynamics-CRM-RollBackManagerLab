using Microsoft.Xrm.Sdk;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using RollBackManagerLab.CustomFlow.CRM.Handler;
using System;
using System.Runtime.ExceptionServices;
using System.Text;

namespace RollBackManagerLab.CustomFlow.Plugins
{
    public class WhenApiKeyCreateCheckPreValidPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext flowContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(flowContext.UserId);
            var logBuilder = new StringBuilder();
            //API key validation

            try
            {
                var apiKeyRecord = flowContext.InputParameters["Target"] as Entity;

                if (apiKeyRecord != null)
                {
                    var key = apiKeyRecord.GetAttributeValue<string>(ApiKeyAttribute.ApiKey);

                    if (!ApiKeyHandler.IsExistSameApiKey(key, service))
                    {
                        var token = CommonOperations.GetAuthToken(apiKeyRecord, ref logBuilder, service, false);

                        if (string.IsNullOrEmpty(token))
                        {
                            throw new InvalidPluginExecutionException("Invalid Api Key Value, Please Check Your Entry !");
                        }
                    }
                    else
                    {
                        throw new InvalidPluginExecutionException("Api Key Is Predefined In The System !");
                    }
                }
                else
                {
                    throw new InvalidPluginExecutionException("Api Key Record Doesn't Exists On Current Context !");
                }
            }
            catch (InvalidPluginExecutionException ex)
            {
                throw ExceptionDispatchInfo.Capture(ex).SourceException;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message ?? ex.InnerException.Message ?? "Unknown Error! LOG : " + logBuilder;
                tracingService.Trace(errorMsg);
            }
        }
    }
}
