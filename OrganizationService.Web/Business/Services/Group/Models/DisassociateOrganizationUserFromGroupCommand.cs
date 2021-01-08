namespace Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models
{
    public class DisassociateOrganizationUserFromGroupCommand
    {
        public int OrganizationId { get; set; }
        public int GroupId { get; set; }
        public int OrganizationUserId { get; set; }
    }
}
