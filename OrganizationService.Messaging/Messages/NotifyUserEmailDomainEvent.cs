namespace Kebormed.Core.OrganizationService.Messaging.Messages
{
    public class NotifyUserEmailDomainEvent
    {
        public string Content { get; set; }
        public string Subject { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientFullName { get; set; }
    }
}