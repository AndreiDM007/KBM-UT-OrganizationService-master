namespace Kebormed.Core.OrganizationService.Web.Business.Services.UserInvitation.Models
{
    public class UpdateUserInvitationCommand
    {
        public string InvitationGuid { get; set; }
        public string ExternalUserId { get; set; }
        public long? AcceptedAt { get; set; }
        public long? DeclinedAt { get; set; }
    }
}