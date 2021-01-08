using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Data.Entities
{
    public partial class OrganizationUserEntity
    {
        public int OrganizationUserEntityId { get; set; }
        public int OrganizationId { get; set; }
        public OrganizationEntity Organization { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public bool IsPendingActivation { get; set; }
        public ProfileEntity Profile { get; set; }
        public int UserType { get; set; }
        public ICollection<AssociatedOrganizationUserEntity> AssociatedOrganizationUsers { get; set; }        
        public ICollection<GroupMemberEntity> Groups { get; set; }
        public ICollection<OrganizationUserRoleEntity> Roles { get; set; }
        
        public UserInvitationEntity UserInvitation { get; set; }
        
        public string TransactionId { get; set; }
        public long? RollbackedAt { get; set; }
        public long? DeletedAt { get; set; }
        public long? CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }

        public long? LastLoginAt { get; set; }
    }
}