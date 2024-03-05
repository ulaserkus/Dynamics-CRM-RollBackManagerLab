using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using RollBackManagerLab.CustomFlow.CRM.Constants;
using RollBackManagerLab.CustomFlow.CRM.Handler;
using System;
using System.Activities;

namespace RollBackManagerLab.CustomFlow.Flows
{
    public class ConfigurationUserManageRoleFlow : CodeActivity
    {

        [Output("Success State")]
        public OutArgument<bool> IsSuccessfull { get; set; }

        [Output("Error Message")]
        public OutArgument<string> ErrorMessage { get; set; }


        [ReferenceTarget(EntityLogicalName.SystemUser)]
        [RequiredArgument]
        [Input("SystemUser")]
        public InArgument<EntityReference> UserRef { get; set; }

        [RequiredArgument]
        [Input("IsAssociation")]
        public InArgument<bool> IsAssociation { get; set; }

        [RequiredArgument]
        [Input("RoleName")]
        public InArgument<string> RoleName { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            IWorkflowContext flowContext = context.GetExtension<IWorkflowContext>();
            ITracingService tracingService = context.GetExtension<ITracingService>();
            IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(flowContext.UserId);

            // Eylemi oluşturun
            try
            {

                var userRef = UserRef.Get<EntityReference>(context);
                var isAssociation = IsAssociation.Get<bool>(context);
                var roleName = RoleName.Get<string>(context);
                var role = RoleHandler.GetRoleByName(roleName, new string[]
                {
                    RoleAttributes.Id,
                    RoleAttributes.Name,
                }, service);

                if (isAssociation)
                {
                    if (SystemUserRoleHandler.UserHasRelatedRole(role.Id, userRef.Id, service))
                    {

                        IsSuccessfull.Set(context, true);
                        ErrorMessage.Set(context, "");

                        return;
                    }

                    service.Associate(
                                    EntityLogicalName.SystemUser,
                                    userRef.Id,
                                    new Relationship(EntityRelationShip.SystemUserRoleNN),
                                    new EntityReferenceCollection() { new EntityReference(EntityLogicalName.Role, role.Id) });
                }
                else
                {

                    if (ConfigurationUserHandler.ActiveConfigurationUserRecordCount(userRef.Id, service) > 1)
                    {
                        IsSuccessfull.Set(context, true);
                        ErrorMessage.Set(context, "");

                        return;
                    }

                    service.Disassociate(
                                   EntityLogicalName.SystemUser,
                                  userRef.Id,
                                   new Relationship(EntityRelationShip.SystemUserRoleNN),
                                   new EntityReferenceCollection() { new EntityReference(EntityLogicalName.Role, role.Id) });
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
