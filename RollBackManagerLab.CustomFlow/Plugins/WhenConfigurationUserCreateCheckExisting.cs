using Microsoft.Xrm.Sdk;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using RollBackManagerLab.CustomFlow.CRM.Handler;
using System;
using System.Runtime.ExceptionServices;
using System.Text;

namespace RollBackManagerLab.CustomFlow.Plugins
{
    public class WhenConfigurationUserCreateCheckExisting : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext flowContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(flowContext.UserId);
            try
            {
                var configurationUserRecord = flowContext.InputParameters["Target"] as Entity;

                if (configurationUserRecord != null)
                {
                    if (!configurationUserRecord.Contains(ConfigurationUserAttribute.UserId))
                        throw new InvalidPluginExecutionException("Invalid Configuration User Value, Please Check Your Entry !");

                    if (!configurationUserRecord.Contains(ConfigurationUserAttribute.ConfigurationId))
                        throw new InvalidPluginExecutionException("Invalid Configuration User Value, Please Check Your Entry !");

                    var configurationRef = configurationUserRecord.GetAttributeValue<EntityReference>(ConfigurationUserAttribute.ConfigurationId);
                    var userRef = configurationUserRecord.GetAttributeValue<EntityReference>(ConfigurationUserAttribute.UserId);

                   
                    if (ConfigurationUserHandler.HasActiveConfigurationUser(configurationRef.Id, userRef.Id, service))
                    {
                        throw new InvalidPluginExecutionException("Invalid Configuration User. You have already registered this user !");
                    }

                }
                else
                {
                    throw new InvalidPluginExecutionException("Configuration User Record Doesn't Exists On Current Context !");
                }
            }
            catch (InvalidPluginExecutionException ex)
            {
                throw ExceptionDispatchInfo.Capture(ex).SourceException;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message ?? ex.InnerException.Message ?? "Unknown Error! LOG : " ;
                tracingService.Trace(errorMsg);
            }
        }
    }
}
