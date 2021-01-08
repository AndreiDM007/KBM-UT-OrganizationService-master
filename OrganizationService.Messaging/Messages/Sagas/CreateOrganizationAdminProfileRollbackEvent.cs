using Kebormed.Core.Saga.Messaging.Messages;

namespace Kebormed.Core.OrganizationService.Messaging.Messages.Sagas
{
    public class CreateOrganizationAdminProfileRollbackEvent : SagaRollbackEvent
    {
        public int OrganizationAdminProfileId { get; set; }

        public string TransactionId { get; set; }
    }
}