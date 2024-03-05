using Microsoft.Xrm.Sdk.Query;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using RollBackManagerLab.CustomFlows.ConsoleApp.Factory;
using System;

namespace RollBackManagerLab.CustomFlows.ConsoleApp
{
    public class Program
    {
        protected Program() { }
        static void Main(string[] args)
        {
            var service = CrmServiceFactory.CreateClientService();

            EOF();
        }


        private static void EOF()
        {
            Console.WriteLine("---------End Of Program----------");
            Console.ReadLine();
        }
    }
}
