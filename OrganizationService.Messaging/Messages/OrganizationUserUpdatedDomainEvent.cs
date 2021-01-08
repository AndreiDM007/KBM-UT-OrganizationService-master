using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Messaging.Messages
{
    public class OrganizationUserUpdatedDomainEvent
    {
        public int OrganizationId { get; set; }
        public int OrganizationUserId { get; set; }
        public Dictionary<int, string> Profile { get; set; }
        public Dictionary<int, int> RelatedOrganizationUsers { get; set; }
        public int UserType { get; set; }
        public string UserId { get; set; } //TODO: ATM there's no use case where it can be updated
        public string Username { get; set; } //TODO: ATM there's no use case where it can be updated
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Roles { get; set; }

        public bool? IsActive { get; set; }
        public long? CreatedAt { get; set; }

        public bool IsPendingActivation { get; set; }
        public bool IsLocked { get; set; }
    }
}