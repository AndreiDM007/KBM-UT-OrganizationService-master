namespace Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models
{
    public class OrganizationUserPermissionCustomModel
    {
        public int OrganizationId { get; set; }
        public int? OrganizationUserId { get; set; }
        public string ExternalUserId { get; set; }
        public string Permissions { get; set; }
        public long? DeletedAt { get; set; }
    }

}