namespace Kebormed.Core.OrganizationService.Web.Business.Services.Organization.Models
{
    public class UpdateOrganizationCommand
    {
        public int? OrganizationId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }
}