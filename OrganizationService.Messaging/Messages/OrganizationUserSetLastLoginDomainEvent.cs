namespace Kebormed.Core.OrganizationService.Messaging.Messages
{
    public class OrganizationUserSetLastLoginDomainEvent
    {
        public string ExternalUserId { get; set; }
        public int UserType { get; set; }
        public long LastLoginAt { get; set; }
    }
}