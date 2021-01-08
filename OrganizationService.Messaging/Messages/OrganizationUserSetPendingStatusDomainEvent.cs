namespace Kebormed.Core.OrganizationService.Messaging.Messages
{
    public class OrganizationUserSetPendingStatusDomainEvent
    {
        public string ExternalUserId { get; set; }
        public int UserType { get; set; }
        public bool IsPendingActivation { get; set; }
    }
}