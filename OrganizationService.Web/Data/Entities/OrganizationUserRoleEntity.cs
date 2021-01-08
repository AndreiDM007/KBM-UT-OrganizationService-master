namespace Kebormed.Core.OrganizationService.Web.Data.Entities
{
    public class OrganizationUserRoleEntity
    {
        public int OrganizationUserRoleEntityId { get; set; }
        public int? OrganizationUserId { get; set; }
        public OrganizationUserEntity OrganizationUser { get; set; }
        public string RoleId { get; set; }
        public int? OrganizationId { get; set; }        
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public long? DeletedAt { get; set; }        
    }
}