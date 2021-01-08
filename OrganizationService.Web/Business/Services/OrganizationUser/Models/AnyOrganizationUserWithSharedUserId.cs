namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models
{
    public class AnyOrganizationUserWithSharedUserId
    {
        public bool IsLastUserIdRelationship { get; set; }
        public string UserId { get; set; }
        public long DeletedAt { get; set; }
    }
}