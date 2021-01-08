using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Data.Entities
{
    public class GroupEntity
    {
        public int OrganizationId { get; set; }
        public int GroupEntityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // FK for parent group
        public int? ParentGroupId { get; set; }
        public GroupEntity ParentGroup { get; set; }
        public ICollection<GroupEntity> SubGroups { get; set; }
        
        public ICollection<GroupMemberEntity> GroupMembers { get; set; }
        
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public long? DeletedAt { get; set; }
    }
}