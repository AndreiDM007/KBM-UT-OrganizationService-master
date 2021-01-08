namespace Kebormed.Core.OrganizationService.Web.Data.Entities
{
    public class RolePermissionEntity
    {
        public int RolePermissionEntityId { get; set; }
        public int? OrganizationId { get; set; }        
        public string RoleId { get; set; }        
        public string RoleName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Permissions { get; set; }
        public bool IsDefault { get; set; }
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public long? DeletedAt { get; set; }
    }
}