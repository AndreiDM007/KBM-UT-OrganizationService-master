namespace Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models
{
    public class QueryOrganizationUserAndRolePermissionCriteria
    {
        public string UserId { get; set; }
        public int? OrganizationId { get; set; }
    }
}