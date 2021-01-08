using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models
{
    public class GetOrganizationUserResult
    {
        public int OrganizationUserId { get; set; }
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public bool IsPendingActivation { get; set; }
        public int ProfileId { get; set; }
        public long? CreatedAt { get; set; }
        public long? LastLoginAt { get; set; }
        
        public int UserType { get; set; } // not to be propagated
        public bool? HasAcceptedInvitation { get; set; }

        public ICollection<ProfileValuesListModel> ProfileValues { get; set; }
        public ICollection<AssociationOrganizationUserListModel> AssociatedOrganizationUsers { get; set; }
        public ICollection<OrganizationUserGroupListModel> Groups { get; set; }
    }
}