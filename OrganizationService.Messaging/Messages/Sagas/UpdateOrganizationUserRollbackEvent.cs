using Kebormed.Core.Saga.Messaging.Messages;

namespace Kebormed.Core.OrganizationService.Messaging.Messages.Sagas
{
    public class UpdateOrganizationUserRollbackEvent : SagaRollbackEvent
    {
        public int OrganizationId { get; set; }
        public int OrganizationUserId { get; set; }
        public int UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string TransactionId { get; set; }
        public bool? IsActive { get; set; }
    }
}