namespace Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models
{
    public class CreateGroupCommand
    {
        public int OrganizationId { get; set; }
        public int? ParentGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
