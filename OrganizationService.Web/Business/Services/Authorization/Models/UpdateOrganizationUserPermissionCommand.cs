namespace Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models
{
    public class UpdateOrganizationUserPermissionCommand
    {
        public int? OrganizationUserId {get; set;}
        public string UserId {get; set;}
        public string Permissions {get; set;}
    }
}