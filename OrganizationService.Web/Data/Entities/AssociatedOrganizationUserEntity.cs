namespace Kebormed.Core.OrganizationService.Web.Data.Entities
{
    public class AssociatedOrganizationUserEntity
    {
        public int AssociatedOrganizationUserEntityId { get; set; }
        public int OrganizationUserId1 { get; set; }
        public int OrganizationUserId2 { get; set; }
        public int AssociationType { get; set; }
        
        public OrganizationUserEntity OrganizationUser { get; set; }
        
        public string TransactionId { get; set; }
        public long? RollbackedAt { get; set; }

        public long CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public long? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public long? DeletedAt { get; set; }
        public string DeletedBy { get; set; }

    }
}