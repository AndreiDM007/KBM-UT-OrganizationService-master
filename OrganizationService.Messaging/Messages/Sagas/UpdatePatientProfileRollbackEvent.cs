using System.Collections.Generic;
using Kebormed.Core.Saga.Messaging.Messages;

namespace Kebormed.Core.OrganizationService.Messaging.Messages.Sagas
{
    public class UpdatePatientProfileRollbackEvent : SagaRollbackEvent
    {
        public int OrganizationId { get; set; }
        public int OrganizationUserId { get; set; }
        public Dictionary<int, string> ProfileValues { get; set; }
        public string TransactionId { get; set; }
    }
}