using Kebormed.Core.Saga.Messaging.Messages;

namespace Kebormed.Core.OrganizationService.Messaging.Messages.Sagas
{
    public class CreateOrganizationRollbackEvent : SagaRollbackEvent
    {
        public int OrganizationId { get; set; }

        public string TransactionId { get; set; }
    }
}