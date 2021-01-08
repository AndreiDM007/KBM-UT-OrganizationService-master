namespace Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models
{
    public class UpdateRolePermissionCommand
    {
        public int? OrganizationId {get; set;}
        public string RoleId {get; set;}
        public string RoleName {get; set;}        
        public string Description { get; set; }
        public string Permissions {get; set;}        
        public string DisplayName { get; set; }
    }
}