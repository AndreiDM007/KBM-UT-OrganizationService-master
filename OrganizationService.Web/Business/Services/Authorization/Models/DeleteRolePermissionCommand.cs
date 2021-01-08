namespace Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models
{
    public class DeleteRolePermissionCommand
    {
        public int? OrganizationId {get; set;}
        public string RoleId {get; set;}
    }
}