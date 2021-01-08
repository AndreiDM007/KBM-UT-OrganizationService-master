namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models
{
    public class UpdateOrganizationUserCommand
    {

        public int OrganizationId { get; set; }
        public int OrganizationUserId { get; set; }
        public int UserType { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }

        public string TransactionId { get; set; }        
    }
}