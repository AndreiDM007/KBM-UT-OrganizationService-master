using Kebormed.Core.Saga.Messaging.Messages;

namespace Kebormed.Core.OrganizationService.Messaging.Messages.Sagas
{
    public class CreateOrganizationUserRollbackEvent : SagaRollbackEvent
    {
        public int OrganizationUserId { get; set; }

        public int UserType { get; set; }
        public int OrganizationId { get; set; }

        public string TransactionId { get; set; }
    }
}