using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Messaging.Messages
{
    public class OrganizationUserAddedDomainEvent
    {
        public int OrganizationId { get; set; }
        public int OrganizationUserId { get; set; }
        public Dictionary<int, string> Profile { get; set; }
        public Dictionary<int, int> RelatedOrganizationUsers { get; set; }
        public int UserType { get; set; }

        public string TransactionId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public bool IsPendingActivation { get; set; }

        public string Roles { get; set; }

        public long? CreatedAt { get; set; }
    }
}