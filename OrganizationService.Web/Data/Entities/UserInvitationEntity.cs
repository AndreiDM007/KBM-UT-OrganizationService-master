namespace Kebormed.Core.OrganizationService.Web.Data.Entities
{
    public class UserInvitationEntity
    {
        public int UserInvitationEntityId { get; set; }
        public int OrganizationId { get; set; }
        public int OrganizationUserId { get; set; }
        public string ExternalUserId { get; set; }
        public string InvitationGuid { get; set; }
        public long CreatedAt { get; set; }
        public long? AcceptedAt { get; set; }
        public long? DeclinedAt { get; set; }
        public long? DeletedAt { get; set; }
        
        public OrganizationUserEntity OrganizationUser { get; set; }
    }
}