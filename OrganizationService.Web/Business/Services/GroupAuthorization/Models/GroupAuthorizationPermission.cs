namespace Kebormed.Core.OrganizationService.Web.Business.Services.GroupAuthorization.Models
{
    public class GroupAuthorizationPermission
    {
        public int TargetOrganizationUserId { get; set; }
        public bool Allowed { get; set; }       
    }
}
