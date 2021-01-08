namespace Kebormed.Core.OrganizationService.Web.Business.Services.Organization.Models
{
    public class CreateOrganizationCommand
    {
        public string Name { get; set; }

        public string TransactionId { get; set; }
        
        public bool IsActive { get; set; }
    }
}