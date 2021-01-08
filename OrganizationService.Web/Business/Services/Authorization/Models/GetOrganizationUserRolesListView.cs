namespace Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models
{
    public class GetOrganizationUserRolesListView
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool IsDefault { get; set; }
    }
}
