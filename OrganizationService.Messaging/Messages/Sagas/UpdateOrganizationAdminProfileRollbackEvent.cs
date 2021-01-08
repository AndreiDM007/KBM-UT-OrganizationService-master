using Kebormed.Core.Saga.Messaging.Messages;

namespace Kebormed.Core.OrganizationService.Messaging.Messages.Sagas
{
    public class UpdateOrganizationAdminProfileRollbackEvent : SagaRollbackEvent
    {
        public int OrganizationUserId { get; set; }
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public int ProfileId { get; set; }
        public long? CreatedAt { get; set; }
    }
}