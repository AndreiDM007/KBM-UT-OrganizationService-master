using Kebormed.Core.Saga.Messaging.Messages;

namespace Kebormed.Core.OrganizationService.Messaging.Messages.Sagas
{
    public class CreateOrganizationUserProfileRollbackEvent : SagaRollbackEvent
    {
        public int OrganizationUserProfileId { get; set; }
        public string TransactionId { get; set; }
    }
}