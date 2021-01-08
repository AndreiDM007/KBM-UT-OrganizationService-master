using Kebormed.Core.Saga.Messaging.Messages;

namespace Kebormed.Core.OrganizationService.Messaging.Messages.Sagas
{
    public class CreateSuperAdminRollbackEvent : SagaRollbackEvent
    {
        public int SuperAdminOrganizationUserId { get; set; }

        public string TransactionId { get; set; }
    }
}