namespace Kebormed.Core.OrganizationService.Web.Business.Services.UserInvitation.Models
{
    public class CreateUserInvitationCommand
    {
        public int OrganizationId { get; set; }
        public int OrganizationUserId { get; set; }
        public string ExternalUserId { get; set; }
    }
}