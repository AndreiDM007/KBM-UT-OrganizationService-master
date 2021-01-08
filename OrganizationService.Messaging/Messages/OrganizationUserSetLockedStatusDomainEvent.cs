namespace Kebormed.Core.OrganizationService.Messaging.Messages
{
    public class OrganizationUserSetLockedStatusDomainEvent
    {
        public string ExternalUserId { get; set; }
        public int UserType { get; set; }
        public bool IsLocked { get; set; }
    }
}