namespace Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models
{
    public class DisassociateOrganizationUserFromAllGroupsCommand
    {
        public int OrganizationId { get; set; }        
        public int OrganizationUserId { get; set; }
    }
}
