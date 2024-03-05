using Microsoft.Xrm.Sdk;
using RollBackManagerLab.CustomFlow.API.Services;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using RollBackManagerLab.CustomFlow.CRM.Handler;
using System;
using System.Text;

namespace RollBackManagerLab.CustomFlow.Plugins
{
    public class WhenApiKeyCreatedPostAsync : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext flowContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(flowContext.UserId);
            var logBuilder = new StringBuilder();

            try
            {
                var apiKeyRecord = flowContext.InputParameters["Target"] as Entity;

                if (apiKeyRecord != null)
                {
                    var key = apiKeyRecord.GetAttributeValue<string>(ApiKeyAttribute.ApiKey);
                    var token = CommonOperations.GetAuthToken(apiKeyRecord, ref logBuilder, service);

                    if (!string.IsNullOrEmpty(token))
                    {
                        var tokens = IdentityApiHandler.GetApiKeys(token, ref logBuilder);
                        var userToken = tokens.Find(x => x.Key.Equals(key));
                        if (userToken != null)
                        {
                            ApiKeyHandler.UpdateApiKeyExpiration(apiKeyRecord.Id, Convert.ToDateTime(userToken.ExpiresInUTC), service);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message ?? ex.InnerException.Message ?? "Unknown Error! LOG : " + logBuilder;
                tracingService.Trace(errorMsg);
            }
        }
    }
}
