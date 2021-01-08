namespace Kebormed.Core.OrganizationService.Web.Data.Entities
{
    public class OrganizationUserPermissionEntity
    {
        public int OrganizationUserPermissionEntityId { get; set; }
        public int? OrganizationUserId { get; set; }
        public string UserId { get; set; }
        public string Permissions { get; set; }
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public long? DeletedAt { get; set; }
    }
}