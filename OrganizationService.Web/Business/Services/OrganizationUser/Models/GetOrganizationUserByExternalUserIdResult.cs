namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models
{
    public class GetOrganizationUserByExternalUserIdResult
    {
        public int OrganizationUserId { get; set; }
        
        public string Username { get; set; }
        
        public int UserType { get; set; }
    }
}