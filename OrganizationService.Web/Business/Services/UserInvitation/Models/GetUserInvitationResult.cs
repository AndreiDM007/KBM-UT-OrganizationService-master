namespace Kebormed.Core.OrganizationService.Web.Business.Services.UserInvitation.Models
{
    public class GetUserInvitationResult
    {
        public int OrganizationId { get; set; } 
        public string OrganizationName { get; set; } 
        public int OrganizationUserId { get; set; } 
        public long CreatedAt { get; set; }
    }
}