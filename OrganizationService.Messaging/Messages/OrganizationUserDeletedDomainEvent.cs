
namespace Kebormed.Core.OrganizationService.Messaging.Messages
{
    public class OrganizationUserDeletedDomainEvent
    {
        public int OrganizationUserId { get; set; }
        public int OrganizationId { get; set; }
        public int UserType { get; set; }
        public long DeletedAt { get; set; }
    }
}
