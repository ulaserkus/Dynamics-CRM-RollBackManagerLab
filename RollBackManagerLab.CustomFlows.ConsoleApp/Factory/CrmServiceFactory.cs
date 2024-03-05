using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;

namespace RollBackManagerLab.CustomFlows.ConsoleApp.Factory
{
    public class CrmServiceFactory
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["MyCDSServer"].ConnectionString;
        protected CrmServiceFactory()
        {

        }
        public static CrmServiceClient CreateClientService()
        {
            return new CrmServiceClient(connectionString);
        }
    }
}
