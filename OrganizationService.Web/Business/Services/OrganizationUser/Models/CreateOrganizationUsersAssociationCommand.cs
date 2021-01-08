namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models
{
    public class CreateOrganizationUsersAssociationCommand
    {
        public int OrganizationId { get; set; }
        public int OrganizationUserId1 { get; set; }
        public int OrganizationUserId2 { get; set; }
        public int AssociationType { get; set; }

        public string TransactionId { get; set; }
    }
}