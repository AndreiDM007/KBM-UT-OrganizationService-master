namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models
{
    public class CreateOrganizationUserCommand
    {
        public string UserId { get; set; }

        public int OrganizationId { get; set; }

        public int UserType { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsPendingActivation { get; set; }

        public string TransactionId { get; set; }
    }
}
