namespace Kebormed.Core.OrganizationService.Web.Data.Entities
{
    public class GroupMemberEntity
    {
        public int GroupMemberEntityId { get; set; }

        public int GroupId { get; set; }
        public int OrganizationUserId { get; set; }
        public int OrganizationId { get; set; }

        public GroupEntity Group { get; set; }
        public OrganizationUserEntity Member { get; set; }
        
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public long? DeletedAt { get; set; }
        
    }
}